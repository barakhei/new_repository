

#include  "msp430x54xA.h"
#include  "lcd_exm.c"

#define CHAT_MODE 1
#define FILE_TRANSFER_GET 2
#define END_MSG 3
#define CONFIG 4
#define RESET 5
#define FILE_from_MEM 6
#define END_OF_ROW 7
#define END_OF_ROW_SW 8
#define MAX_LEN 1000

/*************** Functions Declaration *************************/
void Delay(int num) ;
void change_baudrate(char br);
void send_char_to_pc(char* msg);
void send_ACK(char msg);
void print_communication(int c);

/*************** Global variables *************************/
char FileToPC[MAX_LEN],TX_data[MAX_LEN]; // File to send/get to/from PC 
char* filetopc_pointer;
int temp1_,count=0, county=0,px=0,py=0;
int temp=1;
int offset;   // Offset in FileToPC 
int offset_send;  /* Offset when sending the file (< offset) */
char mode;  
char  ack=0x03,ack1=0x24,c; //ack=" "; ack1=EOF (after sending the file)
char  recived_chat[MAX_LEN];

/*****************MAIN******************************/
void main(void)
{ 
  WDTCTL = WDTPW + WDTHOLD; // Stop WDT
  P5SEL |= 0xC0;  
  count=0; county=0;px=0;py=0;
  /* USCI configuration */
  UCA1CTL0 = 0xD0;          // Parity enable+even parity+MSB first select+7bit+one stop bit select+UART mode+Asynchronous mode
  UCA1CTL1 |= UCSSEL_2;     // Use SMCLK clock
                          
 // Baud rate generation 
  UCA1BR0 = 0x6D;          
  UCA1BR1 = 0x00;
  UCA1MCTL = 0x04;	   
    
  P2SEL |= 0x030; 
  UCA1CTL1 &= ~UCSWRST;   // Use SMCLK clock for USCI
  UCA1IE = 0x01;          // RX interrupt enable
  UCA1IFG = 0x00;
  UCA1TXBUF=0x00; 

  /*Set SW1 + SW2 for input */
  P2IFG = 0x00;          
  P2IES &= 0xC0;         
  P2SEL &= 0x3F;            
  P2IE &= 0x00;           
  P2IE |= 0xC0;
  P2DIR &= 0x3F;
  
  /*Initial LCD settings*/
  loadSettings();			
  halLcdInit();      			
  halLcdBackLightInit();  			
  halLcdSetBackLight(lcdBackLightLevelSettingLOCAL);			
  halLcdSetContrast(lcdContrastSettingLOCAL);			
  halLcdClearScreen();			
  halLcdPrint( "Terminal Project DCS ", 0) ;			
	  
  filetopc_pointer=&FileToPC[0];          //to keep track on where the file is saved
  __bis_SR_register(LPM0_bits + GIE);     // Enter LPM0, interrupts enabled
} //end main


//*******************Interrupt service routine***************************//

#pragma vector=USCI_A1_VECTOR
__interrupt void USCI_A1_ISR (void)
{
  switch(__even_in_range(UCA1IV,4)) //USCI interrupt choosing between 3 cases 0,2,4
  {
    case 0: break;
 
    case 2: //**********------------RX_int---------------------**********//
      
           //first cahr send for config selecting mode
           if ((UCA1RXBUF == 0x05) ) {
              mode = CHAT_MODE;
	      offset_send = 1;            /* Reset offset when sending */
           } else if (UCA1RXBUF == 0x0C) {
              mode = FILE_TRANSFER_GET;
              offset = 0;                 /* Reset offset */ 
           } else if (UCA1RXBUF == 0x0E) {
              mode = END_MSG;
           } else if (UCA1RXBUF == 0x17) {
              mode =   RESET;
           } else if (UCA1RXBUF == 0x15) {
              mode =   FILE_from_MEM;
           } else if (UCA1RXBUF == 0x16) {
              mode = CONFIG;
           } else if (UCA1RXBUF == 0x10) {
              mode = END_OF_ROW;
           } else if (UCA1RXBUF == 0x11) {
              mode = END_OF_ROW_SW;
           }
           
          //change Config mode 
          if ((mode == CONFIG) && (UCA1RXBUF !=0x16)) { 
             temp1_=UCA1RXBUF;
             send_ACK(ack);
             Delay(3200);
             if(UCA1CTL0 & 0x10) temp1_=((temp1_/32)*64)+(temp1_%32);  //converting from 7 bits to 8 bits 
             UCA1CTL0 =(temp1_ & 0xF8);                                // changing ctl reg  without the three last bits
             change_baudrate(UCA1RXBUF & 0x07);                       // Configure baud rate                         
          }          
         //reset screen
          if (mode == RESET){// && (UCA1RXBUF !=0x17)){
             halLcdClearScreen();
             halLcdPrint( "Terminal Project DCS ", 0) ;
             Delay(32000);  
          } 
         //go to next row1
          if (mode == END_OF_ROW){// && (UCA1RXBUF !=0x17)){
             county=county+10;
             count=0;
             //Delay(32000);  
          } 
          
        //go to next row2
          if (mode == END_OF_ROW_SW){// && (UCA1RXBUF !=0x17)){
             py=py+10;
             px=0;
             //Delay(32000);  
          } 
          
        //Chat mode
          if (mode == CHAT_MODE && (UCA1RXBUF !=0x05)){
              c=(char)UCA1RXBUF;
              recived_chat[0]=c;
              recived_chat[1]='\0';
              send_char_to_pc(recived_chat);
              halLcdPrintXY(recived_chat,count,county, 0) ;
              count=count+8;
              Delay(32000);
          } 
	 //end msg from chat
          if (mode == END_MSG) {
              recived_chat[count]='\0';
              send_char_to_pc(recived_chat);
              halLcdClearScreen();
              recived_chat[count-2]='\0'; //for lcd display
              halLcdPrint(recived_chat, 0) ;
               count=0;
                 
          }
         //Receiving file from PC
          if (mode == FILE_TRANSFER_GET && (UCA1RXBUF !=0x0C)) {
              if (UCA1RXBUF == 0x03){ // if we got % == EOF
                  send_ACK(ack1);
                  Delay(32000);
                  halLcdClearScreen();
                  halLcdPrint("File Recived", 0);
              }else if (offset < MAX_LEN) {
                  FileToPC[offset] = UCA1RXBUF;
                  offset++;
              }
              FileToPC[offset]='\n\0';
          }
        //send file from Mem
          if (mode == FILE_from_MEM){
              send_char_to_pc(FileToPC);
              send_ACK(ack);           
              halLcdClearScreen();
              halLcdPrint("File Sent", 0) ;
          }  

  break;
  
  
  case 4://*******-----------------  Tx_int  --------------------*********//  
	UCA1TXBUF = TX_data[0];
	UCA1IE &= ~UCTXIE; //disabling interrupt
	__bic_SR_register(GIE);  //setting off GIE
        
	int i=0;                                                            
        for(i=0; TX_data[i+1]!='\0' ;i++)
            TX_data[i]=TX_data[i+1];

        TX_data[i]='\0';
  break;  
  }
}

#pragma vector=PORT2_VECTOR //SW1 and SW2 interrupt vector
__interrupt void PORT2_ISR(void) 
 {
  //if (mode != END_MSG)
  //{
  //  P2IFG &= 0x00;    //flag off
  //  return;
  //}
  
  if (P2IFG==0x40)      //user pressed SW1
  {
    while(!(UCA1IFG&UCTXIFG));						// wait for UCA1TXBUF to finish transmitting
    UCA1TXBUF = '0';
    halLcdPrintXY( "0",px,50+py, 0) ;
    px=px+8;
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\r';    
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\n';    
  }  
  if (P2IFG==0x80)      //user pressed SW2
  {
    while(!(UCA1IFG&UCTXIFG));						// wait for UCA1TXBUF to finish transmitting
    UCA1TXBUF = '1';   
    halLcdPrintXY( "1",px,50+py, 0) ;
    px=px+8;
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\r';      
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\n';    
 }
  P2IFG &= 0x00;    //flag off
   return; 
 }//end PORT2 ISR
 
//*******************Functions***************************//

void Delay(int num) {
  int i;
  for (i = num ; i > 0; i--);
}//delay function end


void send_ACK(char msg) {

     TX_data[0]=msg;
     TX_data[1]='\0';
     UCA1IE |= UCTXIE;  // TX interrupt enable
     __bis_SR_register(GIE);  
}//senf ACK function end


void send_char_to_pc(char* msg) {
  int i;
  for(i=0;msg[i] != '\0';i++) {                              
     TX_data[0]=msg[i];
     TX_data[1]='\0';
     UCA1IE |= UCTXIE;        // TX interrupt enable
     __bis_SR_register(GIE); //global enable 
     Delay(32000);    
  }
}//Send to PC function end

void change_baudrate(char br) {
  switch(br) {
    case 0x00: //1200
      UCA1CTL1 |= (UCSSEL_1); //using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x1B; // modulation register from table 34-4
      UCA1BR1 = 0x00;
      UCA1MCTL = 0x04;  // modulation register from table 34-4 BRSx
      break;
    case 0x01: //2400
      UCA1CTL1 |= (UCSSEL_1);//using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x0D;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x0C;  // modulation register from table 34-4 BRSx
      break;
    case 0x02: //4800
      UCA1CTL1 |= (UCSSEL_1);//using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x06;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x0E;  // modulation register from table 34-4 BRSx
      break;
    case 0x03: //9600
      UCA1CTL1 |= (UCSSEL_1); //using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x03;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x06;  // modulation register from table 34-4 BRSx
      break;

  default: break;
  }
}//change baudrate function end