#include  "msp430x54xA.h"
#include  "lcd_exm.c"

#define CHAT_MODE 1
#define FILE_TRANSFER_GET 2
#define FILE_TRANSFER_SEND 3
#define End_msg 4
#define CONFIG 5
#define Get_comm_info 6
#define FILE_from_MEM 7
#define MAX_LEN 1000  /* Maximum file length to be received by MSP430 */

/* Communication mode */
//chat mode          - 5
//file transfer send - 6
//file from memory   - 15  = 21
//config             - 16  = 22
// get comm info     - 17  = 23
// file transfer get - 0C  = 12
// end message       - 0Eh = 14
// end of file       - 24h = 36
/* Table of contant:
    init variables
    Delay routine
    Print the comm info on the LCD
    Send character array to PC
    Send Acknowledge to PC
    Change baudrate
    RX and TX interrupt service routine
    
/*************** init variables *************************/
char mode;  // Communication mode
char* file_in_memory= "Terminal Project - Yarden and Nir";
char* filetopc_pointer;
char FileToPC[MAX_LEN],TX_data[MAX_LEN]; // File to send/get to/from PC 
int offset;   // Offset in FileToPC 
int offset_send;  /* Offset when sending the file (< offset) */
char  ack=0x03,ack1=0x24,c; //ack=" "; ack1=EOF (after sending the file)
char  recived_chat[MAX_LEN];
int temp1_,count=0;
int temp=1;

/************ Delay routine *****************/
void Delay(int num) {
  int i;
  for (i = num ; i > 0; i--);
}

/************ Print the comm info on the LCD *************/
void print_communication(int c) {
    int temp_for_lcd;
    halLcdPrint("Comm. Settings:   \0", 0);
  
    /* Print the parity enable bit */
      temp_for_lcd = (c & 0x80); // check if bit[7] set
      if (temp_for_lcd == 0) 
          halLcdPrint("Parity bit:      Disabled         \0", 0);
      else {
      /* Print the parity config bit */
          halLcdPrint("Parity bit:      Enabled and \0", 0);
          temp_for_lcd = (c & 0x40); // check if bit[6] set
          if (temp_for_lcd == 0) 
            halLcdPrint("Odd  \0", 0);
          else {
            halLcdPrint("Even \0", 0);
          }
      }
  
    /* Print the data bit */
      temp_for_lcd = (c & 0x10);  // check if bit[4] set
      if (temp_for_lcd == 0) {
        halLcdPrint("8-bit data       \0", 0);
      } else {
        halLcdPrint("7-bit data       \0", 0);
      }
  
    /* Print the stop bit */
     temp_for_lcd = (c & 0x08); // check if bit[3] set
      if (temp_for_lcd == 0) {
        halLcdPrint("One stop bit     \0", 0);
      } else {
        halLcdPrint("Two stop bits    \0", 0);
      }
  
    /* Print the baudrate */
     temp_for_lcd = (c & 0x07); // check bit[0,1,2] status
      switch(temp_for_lcd) {
      case 0: halLcdPrint("Baud-Rate: 1200 \0", 0);
             break;
      case 1: halLcdPrint("Baud-Rate: 2400 \0", 0);
             break;
      case 2: halLcdPrint("Baud-Rate: 4800 \0", 0);
             break;
     case 3: halLcdPrint("Baud-Rate: 9600 \0", 0);
             break;  
      }
}


/************* Send character array to PC ****************/
void send_char_to_pc(char* msg) {
  int i;
  
  for(i=0;msg[i] != '\0';i++) {                              //itai lo simech aly
     TX_data[0]=msg[i];
     TX_data[1]='\0';
     UCA1IE |= UCTXIE;        // TX interrupt enable
     __bis_SR_register(GIE); //global enable 
     Delay(32000);    
  }
}

/************* Send Acknowledge to PC **************/
void send_ACK(char msg) {

     TX_data[0]=msg;
     TX_data[1]='\0';
     UCA1IE |= UCTXIE;  // TX interrupt enable
     __bis_SR_register(GIE);  
}

/*********** Change baudrate ****************/
/*  0 - 1200 / ACLK
    1 - 2400 / ACLK
    2 - 4800 / ACLK
    3 - 9600 / ACLK
*/
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
}



//***************** RX and TX interrupt service routine ******************************//
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
           } else if (UCA1RXBUF == 0x07) {
	      mode = FILE_TRANSFER_SEND;
	      offset_send = 1;            /* Reset offset when sending */
           } else if (UCA1RXBUF == 0x0C) {
              mode = FILE_TRANSFER_GET;
              offset = 0;                 /* Reset offset */ 
           } else if (UCA1RXBUF == 0x0E) {
              mode = End_msg;
           } else if (UCA1RXBUF == 0x17) {
              mode =   Get_comm_info;
           } else if (UCA1RXBUF == 0x15) {
              mode =   FILE_from_MEM;
           } else if (UCA1RXBUF == 0x16) {
              mode = CONFIG;
           }
          //change Config mode 
          if ((mode == CONFIG) && (UCA1RXBUF !=0x16)) { 
            /*
            temp1_=UCA1CTL0 & 0x10;                    //checking UCA1CTL0[4] data length 
             if (temp1_) {                              //we are in 7 bit mode
                temp1_=UCA1RXBUF;
                send_ACK(ack);
                Delay(3200);
                temp1_=((temp1_/32)*64)+(temp1_%32);  //converting from 7 bits to 8 bits 
                UCA1CTL0 =(temp1_ & 0xF8);            // changing ctl reg  without the three last bits
                change_baudrate(UCA1RXBUF & 0x07);  // Configure baud rate
             }
             else {
                temp1_=UCA1RXBUF;
                send_ACK(ack);
                Delay(3200);
                UCA1CTL0 = (temp1_ & 0xF8);   // changing ctl reg  without the three last bits 
                change_baudrate(UCA1RXBUF & 0x07); // Configure baudrate
             }*/             
             temp1_=UCA1RXBUF;
             send_ACK(ack);
             Delay(3200);
             if(UCA1CTL0 & 0x10) temp1_=((temp1_/32)*64)+(temp1_%32);  //converting from 7 bits to 8 bits 
             UCA1CTL0 =(temp1_ & 0xF8);                                // changing ctl reg  without the three last bits
             change_baudrate(UCA1RXBUF & 0x07);                       // Configure baud rate                         
          }          
         //print Config
          if (mode == Get_comm_info && (UCA1RXBUF !=0x17)){
              temp1_=UCA1CTL0 & 0x10; //checking stop bit 8bit/7bit
              if (temp1_) {
                  temp1_=UCA1RXBUF;
                  temp1_=((temp1_/32)*64)+(temp1_%32); //converting from 7 bits to 8 bits 
                  halLcdClearScreen(); 
                  print_communication(temp1_);
              }
              else {
                  temp1_=UCA1RXBUF;
                  halLcdClearScreen(); 
                  print_communication(temp1_);
              }
              Delay(32000);  
          } 
        //Chat mode
          if (mode == CHAT_MODE && (UCA1RXBUF !=0x05)){
              c=(char)UCA1RXBUF;
              recived_chat[count]=c;
              count++;
          } 
	 //end msg from chat
          if (mode == End_msg) {
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
                  offset++;
              }else if (offset < MAX_LEN) {
                  FileToPC[offset] = UCA1RXBUF;
                  offset++;
              }
              FileToPC[offset]='\0';
          }
        //send file from Mem
          if (mode == FILE_from_MEM){
              send_char_to_pc(file_in_memory);
              send_ACK(ack);           
              halLcdClearScreen();
              halLcdPrint("File Sent", 0) ;
          }  
         //send file recived from PC
          if (mode == FILE_TRANSFER_SEND) {
              send_char_to_pc( FileToPC);
              send_ACK(ack);
              halLcdClearScreen();
              halLcdPrint("File Sent", 0) ;
          }
  break;
  
  
  case 4://*******-----------------  Tx_int  --------------------*********//  
	UCA1TXBUF = TX_data[0];
	UCA1IE &= ~UCTXIE; //disabling interrupt
	__bic_SR_register(GIE);  //setting off GIE
        
	int i=0;                                                            // itai lo somech aly
        for(i=0; TX_data[i+1]!='\0' ;i++)
            TX_data[i]=TX_data[i+1];

        TX_data[i]='\0';
  break;  
  }
}


#pragma vector=PORT2_VECTOR //SW1 and SW2 interrupt vector
__interrupt void PORT2_ISR(void) 
 {
  if (mode != End_msg)
  {
    P2IFG &= 0x00;    //flag off
    return;
  }
  
  if (P2IFG==0x40)      //user pressed SW1
  {
    while(!(UCA1IFG&UCTXIFG));						// wait for UCA1TXBUF to finish transmitting
    UCA1TXBUF = '0';    
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\r';    
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\n';    
	
  }  
  if (P2IFG==0x80)      //user pressed SW2
  {
    while(!(UCA1IFG&UCTXIFG));						// wait for UCA1TXBUF to finish transmitting
    UCA1TXBUF = '1';    
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\r';      
    while(!(UCA1IFG&UCTXIFG));
    UCA1TXBUF = '\n';    
 }
  P2IFG &= 0x00;    //flag off
   return; 
 }
//***************** end interrupt service routine ******************************//



void main(void)
{ 
  WDTCTL = WDTPW + WDTHOLD; // Stop WDT
  P5SEL |= 0xC0;  
  
  /* UART configuration */
  UCA1CTL0 = 0xD0;          // 11010000 Communication properties (parity enable, even parity, MSB first select, character length (8 or 7), stop bit select, USCI mode, Synchronises mode). 
  UCA1CTL1 |= UCSSEL_2;     // USCI clock source is SMCLK
                          
 // Baud rate generation - 9600 - see table 34 - 4 - with SMCLK (1.04 MHZ)
  UCA1BR0 = 0x6D;          //1.04mhz/9600 = 109
  UCA1BR1 = 0x00;
  UCA1MCTL = 0x04;	   //for control purpuses
    
  P2SEL |= 0x030; 
  UCA1CTL1 &= ~UCSWRST;   // **Initialize USCI state machine**
  UCA1IE = 0x01;          // RX interrupt enable
  UCA1IFG = 0x00;
  UCA1TXBUF=0x00; 

  /* SW1 + SW2 config */
  P2IFG = 0x00;           // Clear flags
  P2IES &= 0xC0;          // Low to High both switches
  P2SEL &= 0x3F;            // Pin function (I/O function)
  P2IE &= 0x00;           // Enable P2.6&7 interrupts (Switch 1&2 interrupt enable)
  P2IE |= 0xC0;
  P2DIR &= 0x3F;

  
  //Lcd configuration

  loadSettings();			
  halLcdInit();      			
  halLcdBackLightInit();  			
  halLcdSetBackLight(lcdBackLightLevelSettingLOCAL);			
  halLcdSetContrast(lcdContrastSettingLOCAL);			
  halLcdClearScreen();			
  halLcdPrint( "Terminal Project- Yarden and Nir", 0) ;			
	  
  filetopc_pointer=&FileToPC[0];          //to keep track on where the file is saved
  __bis_SR_register(LPM0_bits + GIE);     // Enter LPM0, interrupts enabled
}