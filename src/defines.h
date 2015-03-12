// defines.h
// 2014-10-13T01:22:00-07:00
namespace FoxTrader
{
    #define nullptr         NULL
	#define LOG_LEVEL 		0			// 0 = No Logging, 1 = Errors, 2 = Exceptions, 3 = Notices, 4 = ALL
	#define FOX_IS			"AWESOME"	// "String Test"
	#define SSTR( x ) dynamic_cast<std::ostringstream &>( ( std::ostringstream() << std::dec << x ) ).str()
}
