#pragma once


#ifdef CPPDLL_EXPORTS
#define CPPDLL __declspec(dllexport)
#else
#define CPPDLL __declspec(dllimport)
#endif // CPP_DLL_EXPORTS

class CPPDLL CppClass
{
public:
	CppClass();
	~CppClass();
	double Add(double src, double dst);
	double Min(double src, double dst);
	double Mut(double src, double dst);
	double Sub(double src, double dst);
private:

};