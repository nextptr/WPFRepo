// CtiScanDeviceEnums.h
//
// Global header file for specifying SMAPI arguments that are enumerations.  Used in communicationg with a
// ScanDevice and for specifying the content of ScanDocuments
//
#pragma once

namespace ScanDeviceEnums	{

	enum DistanceUnit
	{
		DistanceUnit_Inches = 1,
		DistanceUnit_Millimeters = 2
	};

	enum ConnectionStatus
	{
		ConnectionStatus_Connected = 0,
		ConnectionStatus_NotConnected = 1,
		ConnectionStatus_NotAvailableForConnection = 2,
		ConnectionStatus_ConWithError = 3,
		ConnectionStatus_NotConWithError = 4
	};

	enum DeviceStatusCategories
	{
		DeviceStatusCategories_None = 0,
		DeviceStatusCategories_ConnectionStatus = 1,
		DeviceStatusCategories_ScanningStatus = 2,
		DeviceStatusCategories_Temperature = 4,
		DeviceStatusCategories_ShutterStatus = 8,
		DeviceStatusCategories_Interlocks = 16,
		DeviceStatusCategories_DeviceSpecific = 32,
		DeviceStatusCategories_DigitalInputStatus = 64,
		DeviceStatusCategories_DigitalOutputStatus = 128,
		DeviceStatusCategories_AnalogInputStatus = 256,
		DeviceStatusCategories_AnalogOutputStatus = 512,
		DeviceStatusCategories_LaserPositionStatus = 1024
	};
	enum DocumentScanningStatus
	{
		Scanning,
		NotScanning,
		Paused,
		ScanningCompleted
	};
	enum DocumentScanningStatusChangeReason
	{
		None,
		Completed,
		Aborted,
		Disconnected,
		DeviceException
	};
	enum ScriptMessageType
	{
		ReportMessage = 0,
		ErrorMessage = 1
	};
	enum TextLineSpaceStyle
	{
		Factor,
		Exactly
	};
	enum TextHorizontalAlign
	{
		TextHorizontalAlign_Left,
		TextHorizontalAlign_Center,
		TextHorizontalAlign_Right
	};
	enum TextVerticalAlign
	{
		TextVerticalAlign_Top,
		TextVerticalAlign_Center,
		TextVerticalAlign_Bottom
	};
	enum HatchLineBorderGapDirection
    {
        Inward = 0,
        Outward = 1
    };
	enum HatchLineStyle
    {
        Unidirectional = 0,
        DoubleFill = 1,
        Serpentine = 2,
        SerpentineConnected = 3,
        Hatch2Times = 4,
        Hatch3Times = 5
    };
	enum HatchOffsetAlgorithm
    {
        DirectOffset = 0,
        InterferenceIndex = 1
    };
	enum HatchCornerStyle
    {
        Sharp = 0,
        SmoothWithLines = 1,
        SmoothWithArcs = 2
    };
    enum HatchOffsetStyle
    {
        HatchOffsetStyle_OutToInward = 0,
        HatchOffsetStyle_InwardToOut = 1
    };
    enum HelixStyle
    {
        HelixStyle_OutToInward = 0,
        HelixStyle_InwardToOut = 1
    };
	enum CutterCompensationDirection
    {
        Inner = 0,
        Outer = 1,
    };

	enum DataMatrixSize
    {
        S8x18 = 2066,
        S8x32 = 2080,
        S10x10 = 2570,
        S12x12 = 3084,
        S12x26 = 3098,
        S12x36 = 3108,
        S14x14 = 3598,
        S16x16 = 4112,
        S16x36 = 4132,
        S16x48 = 4144,
        S18x18 = 4626,
        S20x20 = 5140,
        S22x22 = 5654,
        S24x24 = 6168,
        S26x26 = 6682,
        S32x32 = 8224,
        S36x36 = 9252,
        S40x40 = 10280,
        S44x44 = 11308,
        S48x48 = 12336,
        S52x52 = 13364,
        S64x64 = 16448,
        S72x72 = 18504,
        S80x80 = 20560,
        S88x88 = 22616,
        S96x96 = 24672,
        S104x104 = 26728,
        S120x120 = 30840,
        S132x132 = 33924,
        S144x144 = 37008
    };

	 enum DataMatrixFormat
    {
        Default = 0,
        Industry = 1,
        Macro05 = 2,
        Macro06 = 3
    };

	enum MarkingOrder
    {
        HatchOnly = 0,
        OutlineOnly = 1,
        HatchBeforeOutline = 2,
        OutlineBeforeHatch = 3,
    };

	enum BarcodeType
    {
        Codabar = 0,
        Code39 = 1,
        Code93 = 2,
        Ean13 = 3,
        Msi = 4,
        Code128 = 5,
        Code128A = 6,
        Code128B = 7,
        Code128C = 8,
        Upca = 9,
        Upce = 10,
        Code11 = 11,
        Ean8 = 12,
        UpcaP2 = 13,
        UpcaP5 = 14,
        Ean13P2 = 15,
        Ean13P5 = 16,
        Ean8P2 = 17,
        Ean8P5 = 18,
        UpceP2 = 19,
        UpceP5 = 20,
        Code2Of5Interleaved = 21,
        Code93FullAscii = 22,
    };

	enum MacroPdf417CompactionMode
    {
        TextMode = 0,
        ByteMode = 1,
        NumericMode = 2
    };

	enum MacroPdf417ErrorCorrectionLevel
    {
        MacroPdf417ErrorCorrectionLevel_Default = 0,
        MacroPdf417ErrorCorrectionLevel_Level1 = 1,
        MacroPdf417ErrorCorrectionLevel_Level2 = 2,
        MacroPdf417ErrorCorrectionLevel_Level3 = 3,
        MacroPdf417ErrorCorrectionLevel_Level4 = 4,
        MacroPdf417ErrorCorrectionLevel_Level5 = 5,
        MacroPdf417ErrorCorrectionLevel_Level6 = 6,
        MacroPdf417ErrorCorrectionLevel_Level7 = 7,
        MacroPdf417ErrorCorrectionLevel_Level8 = 8,
        MacroPdf417ErrorCorrectionLevel_Level0 = 9
    };

	enum MicroQRCodeErrorCorrectionLevel
    {
        MicroQRCodeErrorCorrectionLevel_L = 0,
        MicroQRCodeErrorCorrectionLevel_M = 1,
        MicroQRCodeErrorCorrectionLevel_Q = 2,
        MicroQRCodeErrorCorrectionLevel_H = 3
    };
	enum MicroQRCodeSize
    {
        MicroQRCodeSize_None = 0,
        MicroQRCodeSize_S11x11 = 1,
        MicroQRCodeSize_S13x13 = 2,
        MicroQRCodeSize_S15x15 = 3,
        MicroQRCodeSize_S17x17 = 4
    };

	enum MicroQRCodeEncodingMode
    {
        DefaultEncodingMode = 0,
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 3,
        Kanji = 4
    };

	enum MicroQRCodeMaskPattern
    {
        MicroQRCodeMaskPattern_Mask0 = 0,
        MicroQRCodeMaskPattern_Mask1 = 1,
        MicroQRCodeMaskPattern_Mask2 = 2,
        MicroQRCodeMaskPattern_Mask3 = 3,
        MicroQRCodeMaskPattern_Default = 4
    };

	enum Pdf417CompactionMode
    {
        Pdf417CompactionMode_TextMode = 0,
        Pdf417CompactionMode_ByteMode = 1,
        Pdf417CompactionMode_NumericMode = 2,
    };

	enum Pdf417ErrorCorrectionLevel
    {
        Pdf417ErrorCorrectionLevel_Default = 0,
        Pdf417ErrorCorrectionLevel_Level1 = 1,
        Pdf417ErrorCorrectionLevel_Level2 = 2,
        Pdf417ErrorCorrectionLevel_Level3 = 3,
        Pdf417ErrorCorrectionLevel_Level4 = 4,
        Pdf417ErrorCorrectionLevel_Level5 = 5,
        Pdf417ErrorCorrectionLevel_Level6 = 6,
        Pdf417ErrorCorrectionLevel_Level7 = 7,
        Pdf417ErrorCorrectionLevel_Level8 = 8,
        Pdf417ErrorCorrectionLevel_Level0 = 9
    };

	enum QRCodeErrorCorrectionLevel
    {
        QRCodeErrorCorrectionLevel_L = 0,
        QRCodeErrorCorrectionLevel_M = 1,
        QRCodeErrorCorrectionLevel_Q = 2,
        QRCodeErrorCorrectionLevel_H = 3
    };

	enum QRCodeSize
    {
        QRCodeSize_None = 0,
        QRCodeSize_S21x21 = 1,
        QRCodeSize_S25x25 = 2,
        QRCodeSize_S29x29 = 3,
        QRCodeSize_S33x33 = 4,
        QRCodeSize_S37x37 = 5,
        QRCodeSize_S41x41 = 6,
        QRCodeSize_S45x45 = 7,
        QRCodeSize_S49x49 = 8,
        QRCodeSize_S53x53 = 9,
        QRCodeSize_S57x57 = 10,
        QRCodeSize_S61x61 = 11,
        QRCodeSize_S65x65 = 12,
        QRCodeSize_S69x69 = 13,
        QRCodeSize_S73x73 = 14,
        QRCodeSize_S77x77 = 15,
        QRCodeSize_S81x81 = 16,
        QRCodeSize_S85x85 = 17,
        QRCodeSize_S89x89 = 18,
        QRCodeSize_S93x93 = 19,
        QRCodeSize_S97x97 = 20,
        QRCodeSize_S101x101 = 21,
        QRCodeSize_S105x105 = 22,
        QRCodeSize_S109x109 = 23,
        QRCodeSize_S113x113 = 24,
        QRCodeSize_S117x117 = 25,
        QRCodeSize_S121x121 = 26,
        QRCodeSize_S125x125 = 27,
        QRCodeSize_S129x129 = 28,
        QRCodeSize_S133x133 = 29,
        QRCodeSize_S137x137 = 30,
        QRCodeSize_S141x141 = 31,
        QRCodeSize_S145x145 = 32,
        QRCodeSize_S149x149 = 33,
        QRCodeSize_S153x153 = 34,
        QRCodeSize_S157x157 = 35,
        QRCodeSize_S161x161 = 36,
        QRCodeSize_S165x165 = 37,
        QRCodeSize_S169x169 = 38,
        QRCodeSize_S173x173 = 39,
        QRCodeSize_S177x177 = 40
    };

	enum QRCodeEncodingMode
    {
        QRCodeEncodingMode_DefaultEncodingMode = 0,
        QRCodeEncodingMode_Numeric = 1,
        QRCodeEncodingMode_Alphanumeric = 2,
        QRCodeEncodingMode_Byte = 3,
        QRCodeEncodingMode_Kanji = 4
    };

	enum QRCodeMaskPattern
    {
        QRCodeMaskPattern_Mask0 = 0,
        QRCodeMaskPattern_Mask1 = 1,
        QRCodeMaskPattern_Mask2 = 2,
        QRCodeMaskPattern_Mask3 = 3,
        QRCodeMaskPattern_Mask4 = 4,
        QRCodeMaskPattern_Mask5 = 5,
        QRCodeMaskPattern_Mask6 = 6,
        QRCodeMaskPattern_Mask7 = 7,
        QRCodeMaskPattern_DefaultMaskPattern = 8,
    };

	enum DrillPatternType
    {
        DrillPatternType_JumpAndFire = 0,
        DrillPatternType_PointAndShoot = 1,
        DrillPatternType_Circle = 2,
    };

	enum VelocityCompensationMode
	{
		VelocityCompensationMode_Disabled = 0,
		VelocityCompensationMode_DutyCycle = 1,
		VelocityCompensationMode_Frequency = 2,
		VelocityCompensationMode_Power = 3
	};

	enum PowerPort
    {
        PowerPort_Analog1 = 0,
        PowerPort_Analog2 = 1,
        PowerPort_Digital = 2,
        PowerPort_PulseWidth = 3
    };

	enum DrillPatternExecuteMode
    {
        DrillPatternExecuteMode_WholeShape = 0,
        DrillPatternExecuteMode_PointByPoint = 1,
    };

	enum PixelModulation
    {
        PixelModulation_PulseWidth = 0,
        PixelModulation_Power = 1,
        PixelModulation_LaserOnTime = 2,
        PixelModulation_JumpAndFire = 3,
    };

    enum RasterScanningDirection
    {
        RasterScanningDirection_TopToBottom = 0,
        RasterScanningDirection_BottomToTop = 1,
        RasterScanningDirection_LeftToRight = 2,
        RasterScanningDirection_RightToLeft = 3,
    };

	enum PixelScanningDirection
    {
        PixelScanningDirection_Forward = 0,
        PixelScanningDirection_Backward = 1,
        PixelScanningDirection_Serpentine = 2,
    };

	enum ArcTextAlign
    {
        ArcTextAlign_Baseline = 0,
        ArcTextAlign_Ascender = 1,
        ArcTextAlign_Descender = 2,
        ArcTextAlign_Center = 3
    };

    enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8,
    };

	enum CtiStorageLocation
	{
		HostPC = 0,
		Flash = 1,
		Usb = 2,
		TempOnDevice = 3
	};

	enum JobExecutionMode
	{
		ExecuteOnce,
		ExecuteContinuous
	};
	enum CommandGenerationMode
	{
		ScanPack,
		Traditional
	};

}