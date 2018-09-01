#include  "msp430x54xA.h"
#include  "LCD.h"


#define CHAT_MODE 1
#define FILE_TRANSFER_GET 2
#define FILE_TRANSFER_SEND 3
#define End_msg 4
#define CONFIG 5
#define Get_comm_info 6
#define FILE_from_MEM 7
#define MAX_LEN 1000  /* Maximum file length to be received by MSP430 */

void Delay(int num);
void print_communication(int c);
void send_char_to_pc(char* msg);
void send_ACK(char msg);
void change_baudrate(char br);
void init_comm();
void init_LCD();
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
char mode;  							// Communication mode
char* file_in_memory= "Terminal Project - Omri and Ofer Group 7";
char* filetopc_pointer;
char FileToPC[MAX_LEN],TX_data[MAX_LEN]; // File to send/get to/from PC 
int offset;   							// Offset in FileToPC 
int offset_send;  						// Offset when sending the file (< offset) 
char  ack=0x03,ack1=0x24,c; 			//ack="EOF "; ack1= (after sending the file)
char  recived_chat[MAX_LEN];
int temp1_,count=0;
int temp=1;

//Delay routine
void Delay(int num) {
  int i;
  for (i = num ; i > 0; i--);
}

//Print the comm info on the LCD 
void print_communication(int c) {
    int temp_for_lcd;
    halLcdPrint("Comm. Settings:   \0", 0);
 
    temp_for_lcd = (c & 0x80); // check if bit[7] set-parity enabled/disabled
    if (temp_for_lcd == 0) halLcdPrint("Parity bit:      Disabled         \0", 0);
    else {
        halLcdPrint("Parity bit:      Enabled and \0", 0);
        temp_for_lcd = (c & 0x40); // check if bit[6] set-parity even/odd
        if (temp_for_lcd == 0) halLcdPrint("Odd  \0", 0);
        else halLcdPrint("Even \0", 0);
    }
  
      temp_for_lcd = (c & 0x10);  // check if bit[4] set-7-bit/8-bit
      if (temp_for_lcd == 0) {
        halLcdPrint("8-bit data       \0", 0);
      } else {
        halLcdPrint("7-bit data       \0", 0);
      }
  
    /* Print the stop bit */
     temp_for_lcd = (c & 0x08); // check if bit[3] set -stop bit
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


/************* Send char to PC ****************/
void send_char_to_pc(char* msg) {
  int i;
  
  for(i=0;msg[i] != '\0';i++) {                              
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
    case 0x00: //32k / 27 = 1200
      UCA1CTL1 |= (UCSSEL_1); //using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x1B; // modulation register from table 34-4
      UCA1BR1 = 0x00;
      UCA1MCTL = 0x04;  // modulation register from table 34-4 BRSx
      break;
    case 0x01: // 32k /13 = 2400
      UCA1CTL1 |= (UCSSEL_1);//using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x0D;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x0C;  // modulation register from table 34-4 BRSx
      break;
    case 0x02: // 32k/6 = 4800
      UCA1CTL1 |= (UCSSEL_1);//using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x06;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x0E;  // modulation register from table 34-4 BRSx
      break;
    case 0x03: //32k/3 = 9600
      UCA1CTL1 |= (UCSSEL_1); //using AClk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x03;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x06;  // modulation register from table 34-4 BRSx
      break;
       case 0x04: //19200
      UCA1CTL1 |= (UCSSEL_2); //using smclk 
      UCA1CTL1 &= ~0x80;  //defining UCA1CTL register
      UCA1BR0 = 0x03;
      UCA1BR1 = 0x00; // modulation register from table 34-4
      UCA1MCTL = 0x06;  // modulation register from table 34-4 BRSx
      break;

  default: break;
  }
}

void init_comm(){
	
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
  
  UCA1IFG = 0x00;
  UCA1TXBUF=0x00;
  UCA1IE = 0x01;          // RX interrupt enable  
}

void init_LCD(){
	 loadSettings();			
  halLcdInit();      			
  halLcdBackLightInit();  			
  halLcdSetBackLight(lcdBackLightLevelSettingLOCAL);			
  halLcdSetContrast(lcdContrastSettingLOCAL);			
  halLcdClearScreen();			
  halLcdPrint( "Yoni and Ron's Terminal DCS PROJECT", 0) ;
}