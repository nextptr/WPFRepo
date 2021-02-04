// CtiSMAPIWin32.h
//
// Global header file for interfacing to SMAPI via teh C++ callable wrapper DLL Cti.SMAPI.Win32.dll
// This defines the SMAPI classes used to interface to a ScanDevice and to create and execute ScanDocuments.
//
#pragma once

#include <string>
#include <list>
#include <vector>
#include "CtiScanDeviceEnums.h"

using namespace std;

typedef unsigned int uint;
typedef wchar_t WCHAR; 

namespace ScanDeviceStructs
{
	struct Point3D
	{
		Point3D(float x,float y,float z){X=x;Y=y;Z=z;};
		Point3D(){};
		float X;
		float Y;
		float Z;
	};
	struct StoredScanDocumentJob
	{
		StoredScanDocumentJob(const wchar_t* fileName, ScanDeviceEnums::CtiStorageLocation location)
		{ 
			int len = (int)wcslen(fileName) + 1;
			FileName = new wchar_t[len]; 
			wcscpy_s(FileName, len, fileName); 
			storageLocation = location; 
		};
		StoredScanDocumentJob() { FileName = new wchar_t[1]; FileName[0] = L'\0'; };
		wchar_t* FileName;
		ScanDeviceEnums::CtiStorageLocation storageLocation;

		~StoredScanDocumentJob() { delete[] FileName; }
	};
}

class CtiDeviceStatusSnapshot
{
public:
	CtiDeviceStatusSnapshot();
	~CtiDeviceStatusSnapshot();

	ScanDeviceEnums::ConnectionStatus ConnectionStatus;
	ScanDeviceEnums::DocumentScanningStatus ScanningStatus;
};


#ifdef CTIHARDWARESCANDEVICEWIN32WRAPPER
		#include "WrapperHelper.h"
		using namespace System;
		#define DLLSPEC __declspec( dllexport)
#else
		class CtiHardwareScanDeviceMiddle;
		class CtiScanDocumentMiddle;
		class CtiVectorImageMiddle;
		class CtiCharacterMiddle;
		class CtiTextShapeMiddle;
		class CtiArcShapeMiddle;
		class CtiCircleShapeMiddle;
		class CtiPolylineShapeMiddle;
		class CtiDataMatrixBarcodeShapeMiddle;
		class CtiLinearBarcodeShapeMiddle;
		class CtiMacroPdfBarcodeShapeMiddle;
		class CtiMicroQRCodeBarcodeShapeMiddle;
		class CtiPdfBarcodeShapeMiddle;
		class CtiQRCodeBarcodeShapeMiddle;
		class CtiDrillShapeMiddle;
		class CtiPointAndShootDrillShapePatternMiddle;
		class CtiJumpAndFireDrillShapePatternMiddle;
		class CtiLaserParametersMiddle;
		class CtiEllipseShapeMiddle;
		class CtiHatchShapeMiddle;
		class CtiRasterImageShapeMiddle;
		class CtiSpiralShapeMiddle;
		class CtiDegree3BezierShapeMiddle;
		class CtiDynamicArcTextShapeMiddle;
		class CtiDynamicTextShapeMiddle;
		class CtiTransformMatrix2DMiddle;
		class CtiScanningCompletionStateMiddle;
		class CtiIScannableMiddle;
		class CtiFileDocumentMiddle;
		class CtiFileReaderMiddle;
		class CtiUnicodeRangeMiddle;
		class CtiGroupShapeMiddle;

		//Add middle class above
		#define DLLSPEC
#endif



namespace CtiSMAPIWin32 
{

	#pragma region DLLSPEC CtiIScannable

	class DLLSPEC CtiIScannable
	{
	private:
	public:
		CtiIScannableMiddle* m_pIScannable;
		
		CtiIScannable();
		~CtiIScannable();

	};

	#pragma endregion

	#pragma region DLLSPEC CtiPolylineShape

	class DLLSPEC CtiPolylineShape
	{
	private:
	public:
		CtiPolylineShapeMiddle* m_pPolylineShape;
		
		CtiPolylineShape();
		~CtiPolylineShape();

		void SetClosed(bool closed);
		void SetCutterCompensationWidth(float cutterCompensationWidth);
		void SetCutterCompensationDirection(ScanDeviceEnums::CutterCompensationDirection cutterCompensationDirection);
		void AddVertex(ScanDeviceStructs::Point3D* point);
	};



	#pragma endregion

	#pragma region DLLSPEC CtiCharacter

	class DLLSPEC CtiCharacter
	{
	private:
	public:
		CtiCharacterMiddle* m_pCharacter;
		
		CtiCharacter();
		~CtiCharacter();

		float Angle;
		float ScaleX;
		float ScaleY;
		float ItalicAngle;
		float CharacterGap;
		bool Backward;
		bool UpsideDown;
		bool IncludeBorder;
		UINT16 CharacterUnicode;
		char* FontName;
		float Height;
		ScanDeviceEnums::FontStyle FontStyle;

		void Subscript(float percentageSize, float percentageLowered);
		void Superscript(float percentageSize, float percentageRaised);
		void SetUpsideDown(bool upsideDown);
		void SetBackward(bool backward);
		void SetAngle(float angle);

		void AddHatchPatternLine(float borderGap,ScanDeviceEnums::HatchLineBorderGapDirection borderGapDirection,float lineGap, 
										float lineAngle, float baseX, float baseY,ScanDeviceEnums::HatchLineStyle hatchStyle,bool withOffset, 
										ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
										ScanDeviceEnums::HatchCornerStyle cornerStyle);
		
	};

	#pragma endregion

	#pragma region DLLSPEC CtiTextShape

	class DLLSPEC CtiTextShape
	{
	private:
		CtiCharacter** m_pCtiCharactersBufArr = nullptr;
		int m_Counts = 0;

		void ReleaseCtiCharactersBufArr();
	public:
		CtiTextShapeMiddle* m_pTextShape;
		
		CtiTextShape();
		~CtiTextShape();

		void AddText(const char* text, const char* fontName, ScanDeviceEnums::FontStyle fontStyle, float height, float gap);
		void SetLocation(float x,float y,float z);
		void SetTextBoxWidthHeight(float width,float height);
		void SetDotDurationMicroseconds(int dotDurationMicroseconds);
		void SetAngle(float angle);
		void SetItalicAngle(float angle);
		void SetScaleX(float scaleX);
		void SetScaleY(float scaleY);
		void SetKerning(bool kerning);
		void SetWordWrap(bool wordWrap);
		void SetLineSpace(float lineSpace);
		void SetTextLineSpaceStyle(ScanDeviceEnums::TextLineSpaceStyle textLineSpaceStyle);
		void SetTextHorizontalAlign(ScanDeviceEnums::TextHorizontalAlign textHorizontalAlign);
		void SetTextVerticalAlign(ScanDeviceEnums::TextVerticalAlign textVerticalAlign);
		CtiCharacter** CtiTextShape::GetCharacters(int &counts);
		
		
		void AddHatchPatternLine(float borderGap,ScanDeviceEnums::HatchLineBorderGapDirection borderGapDirection,float lineGap, 
												float lineAngle, float baseX, float baseY,ScanDeviceEnums::HatchLineStyle hatchStyle,bool withOffset, 
												ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
												ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternOffsetFilling(float offsetGap, ScanDeviceEnums::HatchOffsetStyle style, 
		                                         ScanDeviceEnums::HatchOffsetAlgorithm algorithm, 
		                                         ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternOffsetInOut(float insideOffsetGap, int insideOffsetCount,
		                                       float outsideOffsetGap, int outsideOffsetCount, 
		                                       ScanDeviceEnums::HatchOffsetAlgorithm algorithm, 
		                                       ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternHelixFilling(float helixGap, ScanDeviceEnums::HelixStyle style, 
														ScanDeviceEnums::HatchOffsetAlgorithm algorithm, 
														ScanDeviceEnums::HatchCornerStyle cornerStyle);		

		void ClearHatchPatterns();
	};



	#pragma endregion

	#pragma region DLLSPEC CtiArcShape

	class DLLSPEC CtiArcShape
	{
	private:
	public:
		CtiArcShapeMiddle* m_pArcShape;
		
		CtiArcShape();
		~CtiArcShape();

		void SetCenterPoint(float x,float y,float z);
		void SetSweepAngle(float angle);
		void SetRadius(float radius);
		void SetStartAngle(float startAngle);
		void SetMaximumSegmentationError(float maximumSegmentationError);
		void SetCutterCompensationWidth(float cutterCompensationWidth);
		void SetCutterCompensationDirection(ScanDeviceEnums::CutterCompensationDirection cutterCompensationDirection);
	
	};

	#pragma endregion

	#pragma region DLLSPEC CtiCircleShape

	class DLLSPEC CtiCircleShape
	{
	private:
	public:
		CtiCircleShapeMiddle* m_pCircleShape;
		
		CtiCircleShape();
		~CtiCircleShape();

		void SetCenterPoint(float x,float y,float z);
		void SetClockwise(bool clockwise);
		void SetRadius(float radius);
		void SetStartAngle(float startAngle);
		void SetMaximumSegmentationError(float maximumSegmentationError);
		void SetCutterCompensationWidth(float cutterCompensationWidth);
		void SetCutterCompensationDirection(ScanDeviceEnums::CutterCompensationDirection cutterCompensationDirection);
	
	};

	#pragma endregion

	#pragma region DLLSPEC CtiDataMatrixBarcodeShape

	class DLLSPEC CtiDataMatrixBarcodeShape
	{
	private:
	public:
		CtiDataMatrixBarcodeShapeMiddle* m_pDataMatrixBarcodeShape;
		
		CtiDataMatrixBarcodeShape();
		~CtiDataMatrixBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetDataMatrixSize(ScanDeviceEnums::DataMatrixSize dataMatrixSize);
		void SetHeight(float height);
		void SetText(const char* text);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void SetDataMatrixFormat(ScanDeviceEnums::DataMatrixFormat dataMatrixFormat);
		void AutoExpand(bool autoExpand);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		
		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiLinearBarcodeShape

	class DLLSPEC CtiLinearBarcodeShape
	{
	private:
	public:
		CtiLinearBarcodeShapeMiddle* m_pLinearBarcodeShape;
		
		CtiLinearBarcodeShape();
		~CtiLinearBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetBarcodeType(ScanDeviceEnums::BarcodeType barcodeType);
		void SetHeight(float height);
		void SetWidth(float width);
		void SetPrintRatio(float printRatio);
		void SetText(const char* text);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		
		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiMacroPdfBarcodeShape

	class DLLSPEC CtiMacroPdfBarcodeShape
	{
	private:
	public:
		CtiMacroPdfBarcodeShapeMiddle* m_pMacroPdfBarcodeShape;
		
		CtiMacroPdfBarcodeShape();
		~CtiMacroPdfBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetHeight(float height);
		void SetWidth(float width);
		void SetText(const char* text);
		void SetNumberOfRows(int numberOfRows);
		void SetNumberOfColumns(int numberOfColumns);

		void AutoExpand(bool autoExpand);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetCompactMode(ScanDeviceEnums::MacroPdf417CompactionMode compactionMode);
		void SetErrorCorrectionLevel(ScanDeviceEnums::MacroPdf417ErrorCorrectionLevel errorCorrectionLevel);

		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiMicroQRCodeBarcodeShape

	class DLLSPEC CtiMicroQRCodeBarcodeShape
	{
	private:
	public:
		CtiMicroQRCodeBarcodeShapeMiddle* m_pMicroQRCodeBarcodeShape;
		
		CtiMicroQRCodeBarcodeShape();
		~CtiMicroQRCodeBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetHeight(float height);
		void SetText(const char* text);
		void AutoExpand(bool autoExpand);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetMicroQRCodeErrorCorrectionLevel(ScanDeviceEnums::MicroQRCodeErrorCorrectionLevel microQRCodeErrorCorrectionLevel);
		void SetMicroQRCodeSize(ScanDeviceEnums::MicroQRCodeSize microQRCodeSize);
		void SetMicroQRCodeEncodingMode(ScanDeviceEnums::MicroQRCodeEncodingMode microQRCodeEncodingMode);
		void SetMicroQRCodeMaskPattern(ScanDeviceEnums::MicroQRCodeMaskPattern microQRCodeMaskPattern);


		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion
	
	#pragma region DLLSPEC CtiPdfBarcodeShape

	class DLLSPEC CtiPdfBarcodeShape
	{
	private:
	public:
		CtiPdfBarcodeShapeMiddle* m_pPdfBarcodeShape;
		
		CtiPdfBarcodeShape();
		~CtiPdfBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetHeight(float height);
		void SetWidth(float width);
		void SetText(const char* text);
		void AutoExpand(bool autoExpand);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void SetNumberOfRows(int numberOfRows);
		void SetNumberOfColumns(int numberOfColumns);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetCompactMode(ScanDeviceEnums::Pdf417CompactionMode compactionMode);
		void SetErrorCorrectionLevel(ScanDeviceEnums::Pdf417ErrorCorrectionLevel errorCorrectionLevel);


		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiQRCodeBarcodeShape

	class DLLSPEC CtiQRCodeBarcodeShape
	{
	private:
	public:
		CtiQRCodeBarcodeShapeMiddle* m_pQRCodeBarcodeShape;
		
		CtiQRCodeBarcodeShape();
		~CtiQRCodeBarcodeShape();

		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetHeight(float height);
		void SetText(const char* text);
		void AutoExpand(bool autoExpand);
		void SetAngle(float angle);
		void InvertImage(bool invertImage);
		void QuietZone(bool quietZone);
		void FlipHorizontally(bool flipHorizontally);
		void FlipVertically(bool flipVertically);
		void SetQRCodeSize(ScanDeviceEnums::QRCodeSize qrCodeSize);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetErrorCorrectionLevel(ScanDeviceEnums::QRCodeErrorCorrectionLevel errorCorrectionLevel);
		void SetQRCodeEncodingMode(ScanDeviceEnums::QRCodeEncodingMode qrCodeEncodingMode);
		void SetQRCodeMaskPattern(ScanDeviceEnums::QRCodeMaskPattern qrCodeMaskPattern);


		void	CreateCircleHatchPattern(float circleRadius);		
		void CreateDotCircleHatchPattern(float circleRadius, int dotsPerCircle, int dotDuration);
		void CreateDotHatchPattern(int dotDuration);
		void CreateHelixHatchPattern(float lineSpace);
		void CreateLineHatchPattern(float lineSpace, bool vertical, bool serpentine);
	};


	#pragma endregion
	
	#pragma region DLLSPEC CtiLaserParameters

	class DLLSPEC CtiLaserParameters
	{
	private:
	public:
		CtiLaserParametersMiddle* m_pLaserParameters;
		
		CtiLaserParameters();
		~CtiLaserParameters();

		void SetVariableName(const char* variableName);
		void SetRepeatCount(int repeatCount);
		void SetModulationFrequency(float modulationFrequency);
		void SetChannelTwoDutyCycle(float channelTwoDutyCycle);
		void SetChannelOneDutyCycle(float channelOneDutyCycle);
		void SetWobbleOverlapPercentage(float wobbleOverlapPercentage);
		void SetWobbleThickness(float wobbleThickness);
		void SetMarkingPowerPercentage(float markingPowerPercentage);
		void SetJumpSpeed(float jumpSpeed);
		void SetMarkingSpeed(float markingSpeed);
		void SetJumpDelay(int jumpDelay);
		void SetLaserOnDelay(int laserOnDelay);
		void SetLaserOffDelay(int laserOffDelay);
		void SetMarkDelay(int markDelay);
		void SetPolyDelay(int polyDelay);
		void SetPipelineDelay(int pipelineDelay);
		void SetPulseWaveform(int pulseWaveform);
		void SetVelocityCompensationMode(ScanDeviceEnums::VelocityCompensationMode velocityCompensationMode);
		void SetVelocityCompensationLimit(float velocityCompensationLimit);
		void SetVelocityCompensationAggressiveness(float velocityCompensationAggressiveness);
		void SetMaxRadialError(float maxRadialError);
		void SetBreakAngle(float breakAngle);

		//todo methods
		//Serialize
		//Deserialize
	};


	#pragma endregion

	#pragma region DLLSPEC CtiPointAndShootDrillShapePattern

	class DLLSPEC CtiPointAndShootDrillShapePattern
	{
	private:
	public:
		CtiPointAndShootDrillShapePatternMiddle* m_pPointAndShootDrillShapePattern;
		
		CtiPointAndShootDrillShapePattern();
		CtiPointAndShootDrillShapePattern(float laserOnTime);
		CtiPointAndShootDrillShapePattern(float laserOnTime, CtiLaserParameters* laserParameters);

		~CtiPointAndShootDrillShapePattern();

		void SetLaserOnTime(float laserOnTime);
		void SetLaserParameters(CtiLaserParameters* laserParameters);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiJumpAndFireDrillShapePattern

	class DLLSPEC CtiJumpAndFireDrillShapePattern
	{
	private:
	public:
		CtiJumpAndFireDrillShapePatternMiddle* m_pJumpAndFireDrillShapePattern;
		
		CtiJumpAndFireDrillShapePattern();
        CtiJumpAndFireDrillShapePattern(float pulseWidth1, float pulseWidth2, float laserModulationDelay, float laserPulseSkew, float laserOffLag, bool usePulseBurstMode);

		~CtiJumpAndFireDrillShapePattern();

		void SetPulseWidth1(float pulseWidth1);
		void SetPulseWidth2(float pulseWidth2);
		void SetLaserModulationDelay(float laserModulationDelay);
		void SetLaserPulseSkew(float laserPulseSkew);
		void SetLaserOffLag(float laserOffLag);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiDrillShape

	class DLLSPEC CtiDrillShape
	{
	private:
	public:
		CtiDrillShapeMiddle* m_pDrillShape;
		
		CtiDrillShape();
		CtiDrillShape(CtiPointAndShootDrillShapePattern* pointAndShootDrillShapePattern);
		CtiDrillShape(CtiPointAndShootDrillShapePattern* pointAndShootDrillShapePattern,ScanDeviceEnums::DrillPatternExecuteMode drillPatternExecuteMode);
		CtiDrillShape(vector<CtiPointAndShootDrillShapePattern*> &pointAndShootDrillShapePatterns, ScanDeviceEnums::DrillPatternExecuteMode drillPatternExecuteMode);
		CtiDrillShape(CtiJumpAndFireDrillShapePattern* jumpAndFireDrillShapePattern);
		CtiDrillShape(vector<CtiJumpAndFireDrillShapePattern*> &jumpAndFireDrillShapePatterns);

		~CtiDrillShape();

		void SetPattern(CtiPointAndShootDrillShapePattern* pointAndShootDrillShapePattern);
		void SetPattern(CtiJumpAndFireDrillShapePattern* jumpAndFireDrillShapePattern);
		void AddJumpAndFirePoint(ScanDeviceStructs::Point3D* point);
		void AddJumpAndFirePoint(float x, float y, float z);
		void AddPointAndShootPoint(ScanDeviceStructs::Point3D* point);
		void AddPointAndShootPoint(float x, float y, float z);
		void AddCirclePoint(ScanDeviceStructs::Point3D* point, float radius);
		void AddCirclePoint(float x, float y, float z, float radius);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiEllipseShape

	class DLLSPEC CtiEllipseShape
	{
	private:
	public:
		CtiEllipseShapeMiddle* m_pEllipseShape;
		
		CtiEllipseShape();
		~CtiEllipseShape();

		void SetCenterPoint(ScanDeviceStructs::Point3D* centerPoint);
		void SetMajorAxisAngle(float majorAxisAngle);
		void SetMajorLength(float majorLength);
		void SetRatioMinorMajor(float ratioMinorMajor);
		void SetClockwise(bool clockwise);
		void SetStartAngle(float startAngle);
		void SetCutterCompensationWidth(float cutterCompensationWidth);
		void SetCutterCompensationDirection(ScanDeviceEnums::CutterCompensationDirection cutterCompensationDirection);
		void SetMaximumSegmentationError(float maximumSegmentationError);
	};


	#pragma endregion

	#pragma region DLLSPEC CtiHatchShape

	class DLLSPEC CtiHatchShape
	{
	private:
	public:
		CtiHatchShapeMiddle* m_pHatchShape;
		
		CtiHatchShape();
		~CtiHatchShape();

		void AddHatchPatternLine(float borderGap, ScanDeviceEnums::HatchLineBorderGapDirection borderGapDirection, float lineGap,
												float lineAngle, float baseX, float baseY, ScanDeviceEnums::HatchLineStyle hatchStyle,
												bool withOffset, ScanDeviceEnums::HatchOffsetAlgorithm algorithm, ScanDeviceEnums::HatchCornerStyle cornerStyle);		

		void AddHatchPatternOffsetFilling(float offsetGap, ScanDeviceEnums::HatchOffsetStyle style,
												 ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
												 ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternOffsetInOut(float insideOffsetGap, int insideOffsetCount,
											   float outsideOffsetGap, int outsideOffsetCount,
											   ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
											   ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternHelixFilling(float helixGap, ScanDeviceEnums::HelixStyle style,
												ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
												ScanDeviceEnums::HatchCornerStyle cornerStyle);

		void AddHatchPatternLine(float borderGap, ScanDeviceEnums::HatchLineBorderGapDirection borderGapDirection, float lineGap,
										float lineAngle, float baseX, float baseY, ScanDeviceEnums::HatchLineStyle hatchStyle,
										bool withOffset, ScanDeviceEnums::HatchOffsetAlgorithm algorithm, ScanDeviceEnums::HatchCornerStyle cornerStyle, bool applySmoothing);

		void AddHatchPatternOffsetFilling(float offsetGap, ScanDeviceEnums::HatchOffsetStyle style,
												 ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
												 ScanDeviceEnums::HatchCornerStyle cornerStyle, bool applySmoothing);

		void AddHatchPatternOffsetInOut(float insideOffsetGap, int insideOffsetCount,
											   float outsideOffsetGap, int outsideOffsetCount,
											   ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
											   ScanDeviceEnums::HatchCornerStyle cornerStyle, bool applySmoothing);

		void AddHatchPatternHelixFilling(float helixGap, ScanDeviceEnums::HelixStyle style,
												ScanDeviceEnums::HatchOffsetAlgorithm algorithm,
												ScanDeviceEnums::HatchCornerStyle cornerStyle, bool applySmoothing);

		void AddLine(float startX, float startY, float startZ, float endX, float endY, float endZ);
		void AddLine2D(float startX, float startY, float endX, float endY);
		void AddCircle(float centerX, float centerY, float centerZ, float radius);
		void AddCircle2D(float centerX, float centerY, float radius);
		void AddCircle(float centerX, float centerY, float centerZ, float radius, float maximumSegmentationError);
		void AddCircle2D(float centerX, float centerY, float radius, float maximumSegmentationError);
		void AddArc(float centerX, float centerY, float centerZ, float radius, float startAngle, float sweepAngle);
		void AddArc2D(float centerX, float centerY, float radius, float startAngle, float sweepAngle);
		void AddArc(float centerX, float centerY, float centerZ, float radius, float startAngle, float sweepAngle, float maximumSegmentationError);
		void AddArc2D(float centerX, float centerY, float radius, float startAngle, float sweepAngle, float maximumSegmentationError);
		void AddEllipse(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor);
		void AddEllipse2D(float centerX, float centerY, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor);
		void AddEllipse(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float maximumSegmentationError);
		void AddEllipse2D(float centerX, float centerY, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float maximumSegmentationError);
		void AddEllipticalArc(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle);
		void AddEllipticalArc2D(float centerX, float centerY, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle);
		void AddEllipticalArc(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle, float maximumSegmentationError);
		void AddEllipticalArc2D(float centerX, float centerY, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle, float maximumSegmentationError);
		void AddRectangle(float lowerLeftX, float lowerLeftY, float upperRightX, float upperRightY, float angle, float elevation);
		void AddRectangle2D(float lowerLeftX, float lowerLeftY, float upperRightX, float upperRightY, float angle);
		void AddPolyline(ScanDeviceStructs::Point3D** vertices,int count);
		void AddPolygon(ScanDeviceStructs::Point3D** vertices, int count);
		void AddDeg3Bezier(ScanDeviceStructs::Point3D** vertices, int count);
		void AddDeg3Bezier(ScanDeviceStructs::Point3D** vertices,float maximumSegmentationError, int count);

	};


	#pragma endregion
	
	#pragma region DLLSPEC CtiRasterImageShape

	class DLLSPEC CtiRasterImageShape
	{
	private:
		float * m_pEnergyProfile = nullptr;
	public:
		CtiRasterImageShapeMiddle* m_pRasterImageShape;
		
		CtiRasterImageShape();
		~CtiRasterImageShape();

		void SetVariableName(const char* variableName);
		void SetRawImageData(BYTE* rawImageData,int lengthOfBYTEArray);
		void SetPowerPort(ScanDeviceEnums::PowerPort powerPort);
		void SetImageData(const char* filePath);
		void SetImageData(BYTE* memoryStream,int lengthOfBYTEArray);
		void SetLocation(ScanDeviceStructs::Point3D* point);
		void SetRasterLineOffsetX(float  rasterLineOffsetX);
		void SetRasterLineOffsetY(float rasterLineOffsetY);
		void SetHeight(float height);
		void SetWidth(float width);
		void SetAngle(float angle);
		void SetLeadIn(float leadIn);
		void SetLeadOut(float leadOut);
		void SetLeadPixelsColor(int leadPixelsColor);
		float* GetEnergyProfile(int* length);
		void SetEnergyProfile(float* energyProfile,int energyProfileArrayLength);
		void SetDotsPerUnitLengthHorizontal(float dotsPerUnitLengthHorizontal);
		void SetDotsPerUnitLengthVertical(float dotsPerUnitLengthVertical);
		void SetLaserOnTime(int laserOnTime);
		void SetPulsePeriod(float pulsePeriod);
		void SetPixelModulation(ScanDeviceEnums::PixelModulation pixelModulation);
		void SetRasterScanningDirection(ScanDeviceEnums::RasterScanningDirection rasterScanningDirection);
		void SetPixelScanningDirection(ScanDeviceEnums::PixelScanningDirection pixelScanningDirection);
		void SetSettlingTime(float settlingTime);
		void SetLaserOffDelay(float laserOffDelay);
		void SetFunctionName(const char* functionName);
		void SetEnableNonProgressiveMode(bool enableNonProgressiveMode);
		void SetLineCount(int lineCount);
	
		float GetWidth();
		float GetHeight();
		float GetDotsPerUnitLengthHorizontal();
		float GetDotsPerUnitLengthVertical();

	};

	#pragma endregion
	
	#pragma region DLLSPEC CtiSpiralShape

	class DLLSPEC CtiSpiralShape
	{
	private:
	public:
		CtiSpiralShapeMiddle* m_pSpiralShape;
		
		CtiSpiralShape();
		~CtiSpiralShape();

		void SetCenterPoint(ScanDeviceStructs::Point3D* centerPoint);
		void SetPitch(float pitch);
		void SetInnerRadius(float innerRadius);
		void SetOuterRadius(float outerRadius);
		void SetAngle(float angle);
		void SetInnerRotations(int innerRotations);
		void SetOuterRotations(int outerRotations);
		void SetClockwise(bool clockwise);
		void SetOutwards(bool outwards);
		void SetReturnToStart(bool returnToStart);

	};


	#pragma endregion

	#pragma region DLLSPEC CtiDegree3BezierShape

	class DLLSPEC CtiDegree3BezierShape
	{
	private:
	public:
		CtiDegree3BezierShapeMiddle* m_pDegree3BezierShape;
		
		CtiDegree3BezierShape();
		~CtiDegree3BezierShape();

		void SetClosed(bool closed);
		void SetMaximumSegmentationError(float maximumSegmentationError);
		void SetCutterCompensationWidth(float cutterCompensationWidth);
		void SetCutterCompensationDirection(ScanDeviceEnums::CutterCompensationDirection cutterCompensationDirection);
		void AddControlPoint(ScanDeviceStructs::Point3D* controlPoint);

	};


	#pragma endregion
	
	#pragma region DLLSPEC CtiDynamicArcTextShape

	class DLLSPEC CtiDynamicArcTextShape
	{
	private:
	public:
		CtiDynamicArcTextShapeMiddle* m_pDynamicArcTextShape;
		
		CtiDynamicArcTextShape();
		~CtiDynamicArcTextShape();

		void SetCenter(ScanDeviceStructs::Point3D* centerPoint);
		void SetClockwise(bool clockwise);
		void SetAlign(ScanDeviceEnums::ArcTextAlign arcTextAlign);
		void SetCharacterGap(float characterGap);
		void SetFontName(const char* fontName);
		void SetEvaluateVariableTags(bool evaluateVariableTags);
		void SetText(const char* text);
		void SetVariableName(const char* variableName);
		void SetHeight(float height);
		void SetElevation(float elevation);
		void SetDotDurationInMicroseconds(int dotDurationInMicroseconds);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetLineHatchPattern(float lineSpace, float angle, ScanDeviceEnums::HatchLineStyle style);
		void SetRadius(float radius);
		void SetStartAngle(float startAngle);
		
	};

	#pragma endregion	

	#pragma region DLLSPEC CtiDynamicTextShape

	class DLLSPEC CtiDynamicTextShape
	{
	private:
	public:
		CtiDynamicTextShapeMiddle* m_pDynamicTextShape;
		
		CtiDynamicTextShape();
		~CtiDynamicTextShape();

		void SetLocation(ScanDeviceStructs::Point3D* location);
		void SetCharacterGap(float characterGap);
		void SetFontName(const char* fontName);
		void SetEvaluateVariableTags(bool evaluateVariableTags);
		void SetText(const char* text);
		void SetVariableName(const char* variableName);
		void SetHeight(float height);
		void SetDotDurationInMicroseconds(int dotDurationInMicroseconds);
		void SetMarkingOrder(ScanDeviceEnums::MarkingOrder markingOrder);
		void SetLineHatchPattern(float lineSpace, float angle, ScanDeviceEnums::HatchLineStyle style);
		void SetAngle(float angle);
		void SetScaleX(float scaleX);
		void SetScaleY(float scaleY);

	};



	#pragma endregion

	#pragma region DLLSPEC CtiTransformMatrix2D

	class DLLSPEC CtiTransformMatrix2D
	{
	private:
	public:
		CtiTransformMatrix2DMiddle* m_pTransformMatrix2D;
		
		CtiTransformMatrix2D();
		CtiTransformMatrix2D(float angle);
		~CtiTransformMatrix2D();

		void SetM00(float m00);
		void SetM01(float m01);
		void SetM10(float m10);
		void SetM11(float m11);
		void LoadIdentity();
	};

	#pragma endregion
	
	#pragma region DLLSPEC CtiScanningCompletionState

	class DLLSPEC CtiScanningCompletionState
	{
	private:
	public:
		CtiScanningCompletionStateMiddle* m_pScanningCompletionState;
		
		CtiScanningCompletionState();
		~CtiScanningCompletionState();

		void SetBeamHomeEnabled(bool beamHomeEnabled);
		void SetBeamHomePosition(ScanDeviceStructs::Point3D* position);
		void SetDisableLaser(bool disableLaser);
		void SetLaserOn(bool setLaserOn);
	};

	#pragma endregion	

	#pragma region DLLSPEC CtiFileDocument

	class DLLSPEC CtiFileDocument
	{
	private:
		list<CtiIScannable*>* m_pCtiIScannables;

		void ReleaseCtiIScannables();
	public:
		CtiFileDocumentMiddle* m_pFileDocument;
		
		CtiFileDocument();
		~CtiFileDocument();

		bool GetBoundingRectangle(ScanDeviceStructs::Point3D* lowerLeftPoint, ScanDeviceStructs::Point3D* upperRightPoint);
		CtiIScannable* GetScannableObject(int dotDuration);
		CtiIScannable* GetTransformedScannableObject(ScanDeviceEnums::DistanceUnit fitBoxUnit, float fitBoxX, float fitBoxY, float fitBoxWidth, float fitBoxHeight, int dotDuration);
		bool ReleaseCtiIScannable(CtiIScannable* which);
	};

	#pragma endregion

	#pragma region DLLSPEC CtiFileReader

	class DLLSPEC CtiFileReader
	{
	private:
		list<CtiFileDocument*>* m_pCtiFileDocuments;
	public:
		CtiFileReaderMiddle* m_pFileReader;
		
		CtiFileReader();
		~CtiFileReader();

		CtiFileDocument* Read(const char* filePath);
		bool ReleaseCtiFileDocument(CtiFileDocument* which);
	};

	#pragma endregion

	#pragma region DLLSPEC CtiUnicodeRange

	class DLLSPEC CtiUnicodeRange
	{
	private:
	public:
		CtiUnicodeRangeMiddle* m_pUnicodeRange;
		
		CtiUnicodeRange();
		~CtiUnicodeRange();

		wchar_t EndingCharacter;
		wchar_t StartingCharacter;
	};

	#pragma endregion
	
	#pragma region DLLSPEC CtiGroupShape

	class DLLSPEC CtiGroupShape
	{
	private:
	public:
		CtiGroupShapeMiddle* m_pGroupShape;
		
		CtiGroupShape();
		~CtiGroupShape();

		void AddCircle(CtiCircleShape* circleShape);
		void AddSpiralShape(CtiSpiralShape* spiralShape, float maxSegmentationError);

	};


	#pragma endregion



	//TODO add new wrapper classes above
	#pragma region DLLSPEC CtiVectorImage

	class DLLSPEC CtiVectorImage
	{
	private:
	public:
		CtiVectorImageMiddle* m_pVectorImage;
		
		CtiVectorImage();
		~CtiVectorImage();

		//wrap methods
		void SetJumpSpeed(float speed);
		void SetMarkSpeed(float speed);
		void SetMarkDelay(int time);
		void SetRepeatCount(int count);
		void SetChannelOneDutyCycle(float dutyCycle);
		void SetChannelTwoDutyCycle(float dutyCycle);
		void SetJumpDelay(int time);
		void SetLaserOffDelay(int time);
		void SetLaserOnDelay(int time);
		void SetLaserPowerPercentage(float laserPowerPercentage);
		void SetModulationFrequency(float frequency);
		void SetPolyDelay(int time);
		void SetVariablePolyDelayEnabled(bool variablePolyDelayEnabled);
		void SetIsStreamed(bool isStreamed);
		void SetPulseWaveform(int pulseWaveform);
		void SetVelocityCompensationMode(ScanDeviceEnums::VelocityCompensationMode velocityCompensationMode, float limit, float aggressiveness);
		void SetMaxRadialError(float maxRadialError);
		void SetBreakAngle(float breakAngle);
		void EnableWobble(float wobbleOverlapPercentage, float wobbleThickness);
		void DisableWobble();
		void SetLaserPropertyVariable(const char* laserPropertyVariable);
		void SetPipelineDelay(int pipelineDelay);

		ScanDeviceEnums::DistanceUnit GetDistanceUnit();
		void AddDot(float x, float y, float z, int durationOfStay);
		void AddLine(float startX, float startY, float startZ, float endX, float endY, float endZ);
		void AddCircle(CtiCircleShape* circleShape);
		void AddCircle(float centerX, float centerY, float centerZ, float radius);
		void AddTextShape(CtiTextShape* textShape);
		void AddTextShape(CtiTextShape* textShape,float maximumSegmentationError);
		void AddArc(CtiArcShape* arcShape);
		void AddArc(float centerX, float centerY, float centerZ, float radius, float startAngle, float sweepAngle);
		void AddPolygon(ScanDeviceStructs::Point3D** vertices, int count);
		void AddPolyline(ScanDeviceStructs::Point3D** vertices, int count);
		void AddPolyline(CtiPolylineShape* polylineShape);
		void AddBarcodeShape(CtiDataMatrixBarcodeShape* matrixBarcodeShape);
		void AddBarcodeShape(CtiLinearBarcodeShape* linearBarcodeShape);
		void AddBarcodeShape(CtiMacroPdfBarcodeShape* macroPdfBarcodeShape);
		void AddBarcodeShape(CtiMicroQRCodeBarcodeShape* microQRCodeBarcodeShape);
		void AddBarcodeShape(CtiPdfBarcodeShape* pdfBarcodeShape);
		void AddBarcodeShape(CtiQRCodeBarcodeShape* qrCodeBarcodeShape);
		void AddDrillShape(CtiDrillShape* drillShape);
		void AddEllipse(CtiEllipseShape* ellipseShape);
		void AddEllipse(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor);
		void AddEllipticalArc(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle);
		void AddEllipticalArc(float centerX, float centerY, float centerZ, float majorAxisLength, float majorAxisAngle, float ratioMinorMajor, float startAngle, float sweepAngle, float maxSegmentationError);
		void AddHatchShape(CtiHatchShape* hatchShape,float elevation);
		void AddPrecisionCircle(float centerX, float centerY, float centerZ, float radius, float maximumSegmentationError);
		void AddRasterImageShape(CtiRasterImageShape* rasterImageShape);
		void AddRectangle(float x, float y, float width, float height, float angle, float elevation);
		void AddSpiralShape(CtiSpiralShape* spiralShape, float maxSegmentationError);
		void AddDeg3BezierPath(ScanDeviceStructs::Point3D** controlPoints,int count);
		void AddDeg3BezierPath(ScanDeviceStructs::Point3D** controlPoints, float maxSegmentationError,int count);
		void AddDeg3BezierPath(CtiDegree3BezierShape* degree3BezierShape);
		void AddDynamicArcTextShape(CtiDynamicArcTextShape* dynamicArcTextShape);
		void AddDynamicRasterImageShape(CtiRasterImageShape* rasterImageShape);
		void AddDynamicTextShape(CtiDynamicTextShape* dynamicTextShape);
		void AddDynamicBarcodeShape(CtiDataMatrixBarcodeShape* dataMatrixBarcodeShape, const char* variableName);
		void AddDynamicBarcodeShape(CtiLinearBarcodeShape* linearBarcodeShape, const char*  variableName);
		void AddDynamicBarcodeShape(CtiMacroPdfBarcodeShape* macroPdfBarcodeShape, const char*  variableName);
		void AddDynamicBarcodeShape(CtiMicroQRCodeBarcodeShape* microQRCodeBarcodeShape,  const char* variableName);
		void AddDynamicBarcodeShape(CtiPdfBarcodeShape* pdfBarcodeShape, const char* variableName);
		void AddDynamicBarcodeShape(CtiQRCodeBarcodeShape* qRCodeBarcodeShape, const char* variableName);
		void AddScannableObject(CtiIScannable* scannableObject);
		void AddGroupShape(CtiGroupShape* groupShape);

	};

	#pragma endregion 

	#pragma region DLLSPEC CtiScanDocument

	class DLLSPEC CtiScanDocument
	{
	private:
		list<CtiVectorImage*>* m_pCtiVectorImages = nullptr;

		void ReleaseCtiVectorImages();
	public:
		CtiScanDocumentMiddle* m_pScanDocument;
		
		CtiScanDocument();
		~CtiScanDocument();
		#pragma region Enums
		enum JobState
		{
			ScanningRequested,
			Scanning,
			Paused,
			ScanningCompleted,
			Idle
		};
		JobState jobState = ScanningCompleted;
		#pragma endregion
		//wrap methods
		CtiVectorImage* CreateVectorImage(const wchar_t* imageName,ScanDeviceEnums::DistanceUnit distanceUnit);
		void AddScript(const wchar_t* name, const wchar_t* script);
		void StartScanning();
		void StartScanning(ScanDeviceEnums::CtiStorageLocation localJobLocation, const wchar_t* localJobName, ScanDeviceEnums::JobExecutionMode executionMode = ScanDeviceEnums::JobExecutionMode::ExecuteOnce);
		void PauseScanning();
		void ResumeScanning();
		void StopScanning();
		void SendCommand(const char* command, const char** args,int argsCount);
		void SetTransformMatrix2D(CtiTransformMatrix2D* transformMatrix2D);
		void SetAfterCompletion(CtiScanningCompletionState* scanningCompletionState);
		void EmbedFont(const char* fontName, ScanDeviceEnums::FontStyle fontStyle, vector<CtiUnicodeRange*> unicodeRanges);
		void StoreScanDocument(ScanDeviceStructs::StoredScanDocumentJob* jobEntry);
		bool ReleaseCtiVectorImage(CtiVectorImage* which);

		void CurrentJobState(uint status);
		uint GetJobState();
		//wrap events
		__event	 void OnDocumentScanningStatusChangedEvent(unsigned int status, unsigned int changedReason);
		void HandleOnDocumentScanningStatusChangedEvent(unsigned int status, unsigned int changedReason);

		__event	 void OnDocumentScanningStatusChangedComEvent(unsigned int status, unsigned int changedReason);
		void HandleOnDocumentScanningStatusChangedComEvent(unsigned int status, unsigned int changedReason);

		__event	 void OnScriptMessageReceivedEvent(unsigned int messageType, const char* message);
		void HandleOnScriptMessageReceivedEvent(unsigned int messageType, const char* message);
		
		__event	 void OnScriptMessageReceivedComEvent(unsigned int messageType, const char* message);
		void HandleOnScriptMessageReceivedComEvent(unsigned int messageType, const char* message);

		__event	 void OnScriptCommandReceivedEvent(const char* command, vector<const char*> &args);
		void HandleOnScriptCommandReceivedEvent(const char* command, vector<const char*> &args);

		__event	 void OnScriptCommandReceivedComEvent(const char* command, vector<const char*> &args);
		void HandleOnScriptCommandReceivedComEvent(const char* command, vector<const char*> &args);
	};
	#pragma endregion 

	#pragma region DLLSPEC CtiScanDeviceManager

	class DLLSPEC CtiScanDeviceManager
	{
	private:
		CtiHardwareScanDeviceMiddle* m_pCtiHardwareScanDevice = nullptr;
		std::wstring *m_pwBuffer = nullptr;

		vector<std::wstring>* m_pwBufArr = nullptr;
		const wchar_t** m_pwcharBufArr = nullptr;

		CtiDeviceStatusSnapshot* m_deviceStatusSnapshot = nullptr;
		list<CtiScanDocument*>* m_pCtiScanDocuments = nullptr;

		vector<ScanDeviceStructs::StoredScanDocumentJob*>* m_StoredScanDocumentBufArr = nullptr;
		ScanDeviceStructs::StoredScanDocumentJob** m_pStoredScanDocumentEntries = NULL;

		void ResetCtiScanDocumentBufArr();
		void ResetStoredScanDocumentBufArr();
		void InitBuffers();
		void ReleaseBuffers();
		const wchar_t** ReturnWcharBufArr();
		ScanDeviceStructs::StoredScanDocumentJob** ReturnStoredScanDocumentEntries();

	public:
		CtiScanDeviceManager();
		~CtiScanDeviceManager();
		
		ScanDeviceEnums::CommandGenerationMode markingMode;
		//wrap methods
		void SetEnabledStatusCategories(ScanDeviceEnums::DeviceStatusCategories statusCategories);
		void LoadConfiguration();
		void LoadConfiguration(const char* filePath);
		void InitializeHardware();
		CtiDeviceStatusSnapshot* GetDeviceStatusSnapshot(const wchar_t* selectedDevice);
		const wchar_t** GetDeviceList(int& deviceCount);
		const wchar_t* GetDeviceFriendlyName(const wchar_t* deviceUniqueName);
		const wchar_t* GetDeviceClass(const wchar_t* deviceUniqueName);
		void Connect(const wchar_t* selectedDevice);
		void Disconnect(const wchar_t* selectedDevice);
		ScanDeviceEnums::ConnectionStatus GetConnectionStatus(const wchar_t* selectedDevice);
		ScanDeviceEnums::DocumentScanningStatus GetScanningStatus(const wchar_t* selectedDevice);
		CtiScanDocument* CreateScanDocument(const wchar_t* selectedDevice, ScanDeviceEnums::DistanceUnit distanceUnit);
		CtiScanDocument* CreateScanDocumentOffline(ScanDeviceEnums::DistanceUnit distanceUnit);
		CtiScanDocument* CreateScanDocumentOffline(const wchar_t* deviceClassName,ScanDeviceEnums::DistanceUnit distanceUnit);
		ScanDeviceStructs::StoredScanDocumentJob** GetStoredScanDocumentList(const wchar_t* selectedDevice,int& jobCount);
		void RenameStoredScanDocument(const wchar_t* selectedDevice, ScanDeviceStructs::StoredScanDocumentJob* jobEntry, const wchar_t* modifiedName);
		void DeleteStoredScanDocument(const wchar_t* selectedDevice, ScanDeviceStructs::StoredScanDocumentJob* jobEntry);
		bool ReleaseCtiScanDocument(CtiScanDocument* which);

		//wrap events
		__event	 void OnDeviceListChangedEvent(const char* obj, unsigned int e);
		void HandleOnDeviceListChangedEvent(const char* obj, unsigned int e);

		__event	 void OnDeviceStatusChangedEvent(const char* obj, unsigned int e);
		void HandleOnDeviceStatusChangedEvent(const char* obj, unsigned int e);

		__event	 void OnDeviceStatusChangedComEvent(const char* obj, unsigned int e);
		void HandleOnDeviceStatusChangedComEvent(const char* obj, unsigned int e);

		__event	 void OnDeviceInterlockTriggeredEvent(const char* deviceUniqueName, const char* interlockName,const char* message);
		void HandleOnDeviceInterlockTriggeredEvent(const char* deviceUniqueName, const char* interlockName,const char* message);

		__event	 void OnScanDeviceGatewayFailedEvent(const char* message, const char* deviceClass,const char* innerException);
		void HandleOnScanDeviceGatewayFailedEvent(const char* message, const char* deviceClass,const char* innerException);
	};

	#pragma endregion 


	
}