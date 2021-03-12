#pragma once

#ifdef MYDLL_EXPORTS
#define MATHLIBRARY_API __declspec(dllexport)
#else
#define MYDLL_API __declspec(dllimport)
#endif

//ÉùÃ÷º¯Êý
extern "C" MYDLL_API double ADD(double dst, double src);
extern "C" MYDLL_API double MIN(double dst, double src);
extern "C" MYDLL_API double MUL(double dst, double src);
extern "C" MYDLL_API double SUB(double dst, double src);
