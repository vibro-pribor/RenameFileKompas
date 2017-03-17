using System;

namespace KAPITypes
{

	/// <summary>
	/// ���� ����������
	/// </summary>
	public enum DocType 
	{
		/// <summary>
		/// 1- ������ �����������
		/// </summary>
		lt_DocSheetStandart = 1,
		/// <summary>
		/// 2- ������ �������������
		/// </summary>
		lt_DocSheetUser,
		/// <summary>
		/// 3- ��������
		/// </summary>
		lt_DocFragment,
		/// <summary>
		/// 4- ������������
		/// </summary>
		lt_DocSpc,
		/// <summary>
		/// 5- 3d-�������� ������
		/// </summary>
		lt_DocPart3D,
		/// <summary>
		/// 6- 3d-�������� ������
		/// </summary>
		lt_DocAssemble3D,
		/// <summary>
		/// 7- ��������� �������� �����������
		/// </summary>
		lt_DocTxtStandart,
		/// <summary>
		/// 8- ��������� �������� �������������
		/// </summary>
		lt_DocTxtUser,
		/// <summary>
		/// 9- ������������ ������������� ������
		/// </summary>
		lt_DocSpcUser,
    /// <summary>
    /// 10- 3d-�������� ��������������� ������
    /// </summary>
    lt_DocTechnologyAssemble3D,
	}


	/// <summary>
	/// ������� ���������
	/// </summary>
	public enum LtQualSystem 
	{
		/// <summary>
		/// 1 - ����
		/// </summary>
		lt_qsShaft = 1,
		/// <summary>
		/// 2 - ���������
		/// </summary>
		lt_qsHole = 2,
	}


	/// <summary>
	/// ���������
	/// </summary>
	public enum LtQualDir 
	{
		/// <summary>
		/// 1 - ����������������
		/// </summary>
		lt_qdPreferable = 1,
		/// <summary>
		/// 2 - ��������
		/// </summary>
		lt_qdBasic,
		/// <summary>
		/// 3 - ��������������
		/// </summary>
		lt_qdAdditional,
	}


	/// <summary>
	/// ���� ������ ��� LtVariant
	/// </summary>
	public enum LtVariantType 
	{
		/// <summary>
		/// 1 - ������
		/// </summary>
		ltv_Char = 1,
		/// <summary>
		/// 2 - ����
		/// </summary>
		ltv_UChar,
		/// <summary>
		/// 3 - �����
		/// </summary>
		ltv_Int,
		/// <summary>
		/// 4 - ����������� �����
		/// </summary>
		ltv_UInt,
		/// <summary>
		/// 5 - ������� �����
		/// </summary>
		ltv_Long,
		/// <summary>
		/// 6 - ������������
		/// </summary>
		ltv_Float,
		/// <summary>
		/// 7 - ������� ������������
		/// </summary>
		ltv_Double,
		/// <summary>
		/// 8 - ������ 255 ��������
		/// </summary>
		ltv_Str,
		/// <summary>
		/// 9 - ���� �� ������������
		/// </summary>
		ltv_NoUsed,
		/// <summary>
		/// 10 - �������� �����
		/// </summary>
		ltv_Short,
		/// <summary>
		/// 11 - ������ 255 �������� Unicode
		/// </summary>
		ltv_WStr,
	}


	/// <summary>
	/// ���� ����� �������� ������
	/// </summary>
	public enum TextAlign 
	{
		/// <summary>
		/// ����� �������� �����
		/// </summary>
		txta_Left = 0,
		/// <summary>
		/// ����� �������� �������
		/// </summary>
		txta_Center,
		/// <summary>
		/// ����� �������� ������
		/// </summary>
		txta_Right
	}


	
	/// <summary>
	/// ������������ ��������� ����� ���� ������ ���������� ����������
	/// </summary>
	public enum LtNodeType 
	{
		/// <summary>
		/// ������ ������
		/// </summary>
		tn_root,
		/// <summary>
		/// ����� (����������)
		/// </summary>
		tn_dir,
		/// <summary>
		/// �������� (����)
		/// </summary>
		tn_file
	}



	/// <summary>
	/// ���� ������ ������� "�������� �������"
	/// </summary>
	public enum LtRemoteElmSignType 
	{
		/// <summary>
		/// 0 - ����������
		/// </summary>
		re_Circle,
		/// <summary>
		/// 1 - �������������
		/// </summary>
		re_Rectangle,
		/// <summary>
		/// 2 - ����������� �������������
		/// </summary>
		re_Ballon
	}


	/// <summary>
	/// ��� ��������� ������� ��������
	/// </summary>
	public enum ChangeOrderType 
	{
		/// <summary>
		/// ���� ����
		/// </summary>
		co_Top = 1,
		/// <summary>
		/// ���� ���� 
		/// </summary>
		co_Bottom,
		/// <summary>
		/// ����� �������� 
		/// </summary>
		co_BeforeObject,
		/// <summary>
		/// �� ��������
		/// </summary>
		co_AfterObject,
		/// <summary>
		/// �� ������� ������
		/// </summary>
		co_UpLevel,
		/// <summary>
		/// �� ������� �����
		/// </summary>
		co_DownLevel
	}


	public class ldefin2d
	{
		public const int TEXT_LENGTH		= 128;
		public const int MAX_TEXT_LENGTH	= 255;

		public const int ODBC_DB	= 0;
		public const int TXT_DB		= 1;

		public const int TXT_CHAR	= 1;
		public const int TXT_USHORT = 2;
		public const int TXT_SSHORT = 3;
		public const int TXT_SLONG	= 4;
		public const int TXT_ULONG	= 5;
		public const int TXT_LONG	= 6;
		public const int TXT_FLOAT	= 7;
		public const int TXT_DOUBLE = 8;
		public const int TXT_INT	= 9;
		public const int TXT_ALL	= 0;
//		public const string TXT_INDEX  "Index1000"

		public const int stACTIVE		= 0;  //��������� ��� ����, ����, ���������
		public const int stREADONLY		= 1;  //��������� ��� ����, ����
		public const int stINVISIBLE	= 2;  //��������� ��� ����, ����
		public const int stCURRENT		= 3;  //��������� ��� ����, ����
		public const int stPASSIVE		= 1;  //��������� ��� ���������

		// ����������� ��� ������� ksSystemPath
		public const int sptSYSTEM_FILES          = 0;  // ������ ���� �� ������� ��������� ������
		public const int sptLIBS_FILES			      = 1;  // ������ ���� �� ������� ������ ���������
		public const int sptTEMP_FILES			      = 2;  // ������ ���� �� ������� ���������� ��������� ������
		public const int sptCONFIG_FILES		      = 3;  // ������ ���� �� ������� ���������� ������������ �������
		public const int sptINI_FILE			        = 4;  // ������ ������ ��� INI-����� �������
		public const int sptBIN_FILE			        = 5;  // ������ ���� �� ������� ����������� ������ �������
		public const int sptPROJECT_FILES		      = 6;  // ������ ���� �� ������� ���������� kompas.prj
		public const int sptDESKTOP_FILES		      = 7;  // ������ ���� �� ������� ���������� kompas.dsk
		public const int sptTEMPLATES_FILES		    = 8;  // ������ ���� �� ������� �������� ������-����������
		public const int sptPROFILES_FILES		    = 9;  // ������ ���� �� ������� ���������� �������� ������������
    public const int sptWORK_FILES            = 10; // ������ ���� �� ������� "��� ���������"
    public const int sptSHEETMETAL_FILES      = 11; // ������ ���� �� ������� ������ ������
    public const int sptPARTLIB_FILES         = 12; // ������ ���� �� ������� PartLib
    public const int sptMULTILINE_FILES       = 13; // ������ ���� � �������� �������� �����������
    public const int sptPRINTDEVICE_FILES     = 14; // ������ ���� � �������� ������������ ���������/���������
    public const int sptCURR_WORK_FILES       = 15; // ����������� ��������� �����������, � ������� ����������� ��������/���������� ����� � ������� Open|Save
    public const int sptCURR_LIBS_FILES       = 16; // ����������� ��������� �����������, � ������� ����������� ��������/���������� ����� � ������� Open|Save
    public const int sptCURR_SYSTEM_FILES     = 17; // ����������� ��������� �����������, � ������� ����������� ��������/���������� ����� � ������� Open|Save
    public const int sptCURR_PROFILES_FILES   = 18; // ����������� ��������� �����������, � ������� ����������� ��������/���������� ����� � ������� Open|Save
    public const int sptCURR_SHEETMETAL_FILES = 19; // ����������� ��������� �����������, � ������� ����������� ��������/���������� ����� � ������� Open|Save

		// ����������� ��� ���������� ������� SystemControlStart
		public const int scsSTOPPED_FOR_MENU_COMMAND		= ( 1); // ��������� ������� ���� "���������� ������ ����������"
		public const int scsSTOPPED_FOR_SYSTEM_STOP			= ( 0); // ���� �������� ������
		public const int scsSTOPPED_FOR_ITSELF				= (-1); // ����� ������� SystemControlStop ��-��� ����������
		public const int scsSTOPPED_FOR_START_THIS_LIB		= (-2); // �������������� ������� ��� ������� ��� �� ����������		
		public const int scsSTOPPED_FOR_START_ANOTHER_LIB	= (-3); // �������������� ������� ��� ������� ������ ����������		

		//  ����������� ��� ������� GetObjParam � SetObjParam
		//  '+'  �������� �������, ��� ������� �����������  GetObjParam � SetObjParam
		public const int ALLPARAM					= -1;  // ��� ��������� ������� � �� ������� ���������
		public const int SHEET_ALLPARAM              = -2;  // ���� ��� �  ALLPARAM  �� ��������� ������� � �� �����
		public const int NURBS_CLAMPED_ALLPARAM      = -5;  // ��������� ������, ������������� ������� ������ � �������  
		public const int NURBS_CLAMPED_SHEET_ALLPARAM= -6;  // ��������� ������ � �� �����, ������������� ������� ������ � �������
		public const int VIEW_ALLPARAM               = -7;  // ��� ��������� ������� � �� ����

		public const int ANGLE_ARC_PARAM       =  0;   // ��������� ���� �� ����� ( ��� ���� � ������������� ���� ) � �� ������� ���������
		public const int POINT_ARC_PARAM       =  1;   // ��������� ���� �� ������ ( ��� ���� � ������������� ���� ) � �� ������� ���������
		public const int ANGLE_ARC_SHEET_PARAM =  2;   // ��������� ���� �� ����� ( ��� ���� � ������������� ���� ) � �� �����
		public const int POINT_ARC_SHEET_PARAM =  3;   // ��������� ���� �� ������ ( ��� ���� � ������������� ���� ) � �� �����
		public const int ANGLE_ARC_VIEW_PARAM  =  4;   // ��������� ���� �� ����� ( ��� ���� � ������������� ���� ) � �� ����
		public const int POINT_ARC_VIEW_PARAM  =  5;   // ��������� ���� �� ������ ( ��� ���� � ������������� ���� ) � �� ����

		public const int VIEW_LAYER_STATE			= 1;   // ��������� ���� ,����
		public const int DOCUMENT_STATE				= 1;   // ��������� ���������
		public const int DOCUMENT_SIZE				= 0;   // ������ �����
		public const int DIM_TEXT_PARAM				= 0;   // ��������� ������ ��� ��������
		public const int DIM_SOURSE_PARAM			= 1;   // ��������� �������� �������
		public const int DIM_DRAW_PARAM				= 2;   // ��������� ��������� �������
		public const int DIM_VALUE					= 3;   // �������� ������� - double
		public const int DIM_PARTS					= 4;   // ������������ ����� ��� �������� struct DimensionPartsParam
		public const int SHEET_DIM_PARTS			= 5;   // ������������ ����� ��� �������� struct DimensionPartsParam � �� �����
		public const int TECHNICAL_DEMAND_PAR		= -1;  // ��������� ����������� ���������� -
		public const int TT_FIRST_STR				= 1000;// ������ ������� ��� ��������� ��� ������ ������ �� �� �������
		public const int CONIC_PARAM				= 2;   // ��������� ��� ���������� ����������� ������� ( ��� ������� � ������������� ���� )
		public const int SPC_TUNING_PARAM			= 0;   // ��������� �������� ��� ����� ��
		public const int HATCH_PARAM_EX				= 0;   // ��������� ��������� �����������
		public const int ASSOCIATION_VIEW_PARAM		= 0;   // ��������� �������������� ����
		public const int DIM_SOURSE_VIEWPARAM     = 7;   // ��������� �������� ������� � ������ ��������� ����
		public const int DIM_DRAW_VIEWPARAM       = 8;   // ��������� ��������� ������� � ������ ��������� ����
		public const int DIM_SOURSE_SHEETPARAM    = 9;   // ��������� �������� ������� � ������ ��������� �����
		public const int DIM_DRAW_SHEETPARAM      = 10;  // ��������� ��������� ������� � ������ ��������� �����

		public const int ALL_OBJ				= 0;         // ��� �������,����� ���������������, ������� ����� ������� � ���                    -
		public const int LINESEG_OBJ			= 1;         // �������                        +
		public const int CIRCLE_OBJ				= 2;         // ����������                     +
		public const int ARC_OBJ				= 3;         // ����                           +
		public const int TEXT_OBJ				= 4;         // �����                          +
		public const int POINT_OBJ				= 5;         // �����                          +
		public const int HATCH_OBJ				= 7;         // ���������                      +
		public const int BEZIER_OBJ				= 8;         // bezier ������                  +
		public const int LDIMENSION_OBJ			= 9;         // �������� ������                +
		public const int ADIMENSION_OBJ			= 10;        // ������� ������                 +
		public const int DDIMENSION_OBJ			= 13;        // ������������� ������           +
		public const int RDIMENSION_OBJ			= 14;        // ���������� ������              +
		public const int RBREAKDIMENSION_OBJ	= 15;        // ���������� ������ � �������    +
		public const int ROUGH_OBJ				= 16;        // �������������                  +
		public const int BASE_OBJ				= 17;        // ����                           +
		public const int WPOINTER_OBJ			= 18;        // ������� ����                   +
		public const int CUT_OBJ				= 19;        // ����� �������                  +
		public const int LEADER_OBJ				= 20;        // ������� ����� �������          +
		public const int POSLEADER_OBJ			= 21;        // ����� ������� ��� ����������� �������      +
		public const int BRANDLEADER_OBJ		= 22;        // ����� ������� ��� ����������� ���������    +
		public const int MARKERLEADER_OBJ		= 23;        // ����� ������� ��� ����������� ������������ +
		public const int TOLERANCE_OBJ			= 24;        // ������ �����                   +
		public const int TABLE_OBJ              = 25;        // �������                        -     //������
		public const int CONTOUR_OBJ            = 26;        // ������                         +     //�����
		public const int MACRO_OBJ              = 27;        // ���������������� ������������  -
		public const int LINE_OBJ               = 28;        // �����                          +
		public const int LAYER_OBJ              = 29;        // ����                           +
		public const int FRAGMENT_OBJ           = 30;        // �������� ��������              +
		public const int POLYLINE_OBJ           = 31;        // ���������                      +
		public const int ELLIPSE_OBJ            = 32;        // ������                         +
		public const int NURBS_OBJ              = 33;        // nurbs ������                   +
		public const int ELLIPSE_ARC_OBJ        = 34;        // ���� �������                   +
		public const int RECTANGLE_OBJ          = 35;        // �������������                  +
		public const int REGULARPOLYGON_OBJ     = 36;        // �������������                  +
		public const int EQUID_OBJ              = 37;        // ������������                   +
		public const int LBREAKDIMENSION_OBJ    = 38;        // �������� ������ � �������      +
		public const int ABREAKDIMENSION_OBJ    = 39;        // ������� ������ � �������       +
		public const int ORDINATEDIMENSION_OBJ  = 40;        // ������ ������
		public const int COLORFILL_OBJ          = 41;        // ������� ������� ������         +
		public const int CENTREMARKER_OBJ       = 42;        // ����������� ������             +
		public const int ARCDIMENSION_OBJ       = 43;        // ������ ����� ����
		public const int SPC_OBJ                = 44;        // ������ ������������            +
		public const int RASTER_OBJ             = 45;        // ��������� ������               +
		public const int CHANGE_LEADER_OBJ      = 46;        // ����������� ���������          -
		public const int REMOTE_ELEMENT_OBJ     = 47;        // �������� �������               +
		public const int AXISLINE_OBJ           = 48;        // ������ �����                   +
		public const int OLEOBJECT_OBJ          = 49;        // ������� ole �������            -
    public const int KNOTNUMBER_OBJ         = 50;        // ������ ����� ����              -
    public const int BRACE_OBJ              = 51;        // ������ �������� ������         -
    public const int POSNUM_OBJ             = 52;        // �����/����������� ����������� � ������-�������� - 
    public const int MARKONLDR_OBJ          = 53;        // �����/����������� ����������� �� �����          -
    public const int MARKWOLDR_OBJ          = 54;        // �����/����������� ����������� ��� �����-������� -
    public const int WAVELINE_OBJ           = 55;        // ��������� �����                -
    public const int DIRAXIS_OBJ            = 56;        // ������ ���                     -
    public const int BROKENLINE_OBJ         = 57;        // ����� ������ � ��������        -
    public const int CIRCLEAXIS_OBJ         = 58;        // �������� ���                   -
    public const int ARCAXIS_OBJ            = 59;        // ������� ���                    -
    public const int CUTUNITMARKING         = 60;        // ����������� ���� � �������     -
    public const int UNITMARKING            = 61;        // ����������� ����      -
    public const int MULTITEXTLEADER        = 62;        // �������� ������� � ������������ ������������.      -
    public const int EXTERNALVIEW_OBJ       = 63;        // ������� �������� ����                              -
    public const int ANNLINESEG_OBJ         = 64;        // ������������� �������                 +- ��� GetObjParam ������������ ��������� LineSegParam
    public const int ANNCIRCLE_OBJ          = 65;        // ������������� ����������              +- ��� GetObjParam ������������ ��������� CircleParam
    public const int ANNELLIPSE_OBJ         = 66;        // ������������� ������                  +- ��� GetObjParam ������������ ��������� EllipseParam
    public const int ANNARC_OBJ             = 67;        // ������������� ����                    +- ��� GetObjParam ������������ ��������� ArcParam
    public const int ANNELLIPSE_ARC_OBJ     = 68;        // ������������� ���� �������            +- ��� GetObjParam ������������ ��������� EllipseArcParam
    public const int ANNPOLYLINE_OBJ        = 69;        // ������������� ���������               +- ��� GetObjParam ������������ ��������� PolylineParam
    public const int ANNPOINT_OBJ           = 70;        // ������������� �����                   +- ��� GetObjParam ������������ ��������� PointParam
    public const int ANNTEXT_OBJ            = 71;        // ����� � ������������� ������ �������� +- ��� GetObjParam ������������ ��������� TextParam
    public const int MULTILINE_OBJ          = 72;        // �����������                    -
    public const int BUILDINGCUTLINE_OBJ    = 73;        // ����� �������/������� ��� ���� + ������������ ��������� CutLineParam
    public const int ATTACHED_LEADER_OBJ    = 74;        // �������������� ����� ������� ( �� ����� ������� )  +
    public const int CONDITIONCROSSING_OBJ  = 75;        // �������� �����������           -
    public const int REPORTTABLE_OBJ        = 76;        // ������������� ������� ������
    public const int EMBODIMENTSTABLE_OBJ   = 77;        // ������� ����������
    public const int SPECIALCURVE_OBJ       = 78;        // ������ ������ ����
    public const int ARRAYPARAMTABLE_OBJ    = 79;        // ������� ���������� �������

		public const int MAX_VIEWTIP_SEARCH     = 80;        // ������� ������� ����� ������ ��� �������� ����  -

		public const int SPECIFICATION_OBJ      = 121;       // ������������ �� �����
		public const int SPECROUGH_OBJ          = 122;       // ����������� �������������      +
		public const int VIEW_OBJ               = 123;       // ���                            +
		public const int DOCUMENT_OBJ           = 124;       // ��������  �����������          +   (���� ��� ��������)
		public const int TECHNICALDEMAND_OBJ    = 125;       // ����������� ����������         +
		public const int STAMP_OBJ              = 126;       // �����                          -  //������
		public const int SELECT_GROUP_OBJ       = 127;       // ������ ��������������          -
		public const int NAME_GROUP_OBJ         = 128;       // ������� ������                 -
		public const int WORK_GROUP_OBJ         = 129;       // ������� ������                 -
		public const int SPC_DOCUMENT_OBJ       = 130;       // ��������  ������������         +
		public const int D3_DOCUMENT_OBJ        = 131;       // 3d ��������  ������ ��� ������ +
		public const int CHANGE_LIST_OBJ        = 132;       // ������� ���������              -
		public const int TXT_DOCUMENT_OBJ       = 133;       // ��������� ��������
		public const int ALL_DOCUMENTS          = 134;       // ��������� ���� �����

		public const int MAX_TIP_SEARCH         = 134;       // ������� ������� ����� ������   -
    public const int ALL_OBJ_SHOW_ORDER     = -1000;     // ��� ������� ������� ����� ������� � ��� � ������� ���������


    // ���� ��� ����� ����� ��������( ��������� ����� ) �� ksCurveStyleEnum:
		//	1  - ��������,
		//  2  - ������,
		//  3  - ������,
		//  4  - ���������,
		//  5  - ��������� �����
		//	6  - ���������������,
		//  7  - ����������,
		//  8  - �����-������� � 2 �������,
		//  9  - ��������� �������
		//  10 -������ �������
		//  11 -������, ���������� � ���������
		//  12 - ISO ��������� �����
		//  13 - ISO ��������� ����� (��. ������)
		//  14 - ISO ��������������� ����� (��. �����)
		//  15 - ISO ��������������� ����� (��. ����� 2 ��������)
		//  16 - ISO ��������������� ����� (��. ����� 3 ��������)
		//  17 - ISO ���������� �����
		//  18 - ISO ��������������� ����� (��. � ���. ������)
		//  19 - ISO ��������������� ����� (��. � 2 ���. ������) 
		//  20 - ISO ��������������� �����
		//  21 - ISO ��������������� ����� (2 ������)
		//  22 - ISO ��������������� ����� (2 ��������)
		//  23 - ISO ��������������� ����� (3��������)
		//  24 - ISO ��������������� ����� (2 ������ 2 ��������)
		//  25 - ISO ��������������� ����� (2 ������ 3 ��������)

		// ���� ��� ����� ��� �����( ��������� ����� ) :
		//0 - �����
		//1 - �������
		//2 - �-�����
		//3	-	�������
		//4	-	�����������
		//5	-	����������
		//6	-	������
		//7	-	������������� �������
		//8	-	���������� ����

    // ���� ��� ��������� ��� ���������( ��������� ����� ) �� ksHatchStyleEnum:
		// 0  - ������
		// 1  - �������� 
		// 2  - ������
		// 3  - ������ ������������
		// 4  - ��������
		// 5  - �����
		// 6  - ������
		// 7  - ��������
		// 8  - ������������ �����
		// 9  - �������� �����
		// 10 - ������ �������������
		// 11 - �����������
		// 12 - ����������� �����������
		// 13 - ������ � ���������� �������
		// 14 - �����

		// ����������� ������ ��� ������
		public const int INVARIABLE			= 0;    //�� ������ ����� ������

		public const int NUMERATOR			= 0x1;    //���������
		public const int DENOMINATOR		= 0x2;    //�����������
		public const int END_FRACTION       = 0x3;    //����� �����
		public const int UPPER_DEVIAT       = 0x4;    //������� ����������
		public const int LOWER_DEVIAT       = 0x5;    //������ ����������
		public const int END_DEVIAT         = 0x6;    //�����  ����������
		public const int S_BASE             = 0x7;    //��������� ��������� ���� �����
		public const int S_UPPER_INDEX      = 0x8;    //������� ������ ��������� ���� �����
		public const int S_LOWER_INDEX      = 0x9;    //������ ������ ��������� ���� �����
		public const int S_END              = 0x10;   //����� ��������� ���� �����
		public const int SPECIAL_SYMBOL     = 0x11;   //��������
		public const int SPECIAL_SYMBOL_END = 0x12;   //��� ���������� � �������
		public const int RETURN_BEGIN       = 0x13;   //������ ��� ����� ��������� ����� � ��������� � �������, ������, �����������
		public const int RETURN_DOWN        = 0x14;   //��� ����� ��������� ����� � ��������� � �������, ������, �����������
		public const int RETURN_RIGHT       = 0x15;   //��� ����� ����� ������ � ��������� � �������, ������, �����������
		public const int TAB                = 0x16;   //��������� �� �������� �����
		public const int FONT_SYMBOL        = 0x17;   //������ �����
    public const int MARK_SEPARATOR     = 0x18;   //����������� � �����������
    public const int FONT_SYMBOL_W      = 0x2017; //������ ����� Unicode
    public const int HYPER_TEXT         = 0x2000; //������ �� ����� ��� ��������� �������

		public const int ITALIC_ON      = 0x40;   //�������� ������
		public const int ITALIC_OFF     = 0x80;   //�������� ������
		public const int BOLD_ON        = 0x100;  //�������� �������
		public const int BOLD_OFF       = 0x200;  //�������� �������
		public const int UNDERLINE_ON   = 0x400;  //�������� �������������
		public const int UNDERLINE_OFF  = 0x800;  //�������� �������������
		public const int NEW_LINE       = 0x1000; //����� ������ � ���������

		public const int FONT_NAME       = 1;       //��� �����
		public const int NARROWING       = 2;       //����������� ������� �����
		public const int HEIGHT          = 3;       //������ �����
		public const int COLOR           = 4;       //���� ������
		public const int SPECIAL         = 5;       //��������
		public const int FRACTION_TYPE   = 6;       //������ ����� �� ��������� � ������ 1-������ ������ 2-� 1.5 ���� ������ 3-� 2 ���� ������
		public const int SUM_TYPE        = 7;       //������ ��������� ���� ����� �� ��������� � ������ 1-������ ������ 2-� 1.5 ���� ������

		//����������� ��� ������������ ��������
		public const int CHAR_STR_ARR          = 1;  // ������������ ������ ���������� �� ������ ��������
		public const int POINT_ARR             = 2;  // ������������ ������ ���������� �� �������������� ����� -��������� MathPointParam
		public const int CURVE_PATTERN_ARR     = 2;  // ������������ ������ ���������� �� ������� ��������� ����� -��������� CurvePattern
		public const int TEXT_LINE_ARR         = 3;  // ������������ ������ ����� ������ - ��������� TextLineParam
		public const int TEXT_ITEM_ARR         = 4;  // ������������ ������ ��������� ����� ������ ��������� TextItemParam
		public const int ATTR_COLUMN_ARR       = 5;  // ������������ ������ ������� ���������- ���������  ColumnInfo
		public const int USER_ARR              = 6;  // ������������ ���������������� ������
		public const int POLYLINE_ARR          = 7;  // ������������ ������ ���������-(���������� �������� POINT_ARR)
		public const int RECT_ARR              = 8;  // ������������ ������ ���������� ���������������-(��������� RectParam)
		public const int LIBRARY_STYLE_ARR     = 9;  // ������������ ������ �������� ���������� ��� ����� � ���������� ������( LibraryStyleParam )
		public const int VARIABLE_ARR          = 10; // ������������ ������ �������� ���������� ��������������� ����������( VariableParam )
		public const int CURVE_PATTERN_ARR_EX  = 11; // ������������ ������ ���������� �� ������� ��������� ����� -��������� CurvePatternEx
		public const int LIBRARY_ATTR_TYPE_ARR = 12; // ������������ ������ �������� ���������� ��� ���� �������� � ���������� ����� ���������( LibraryAttrTypeParam )
		public const int NURBS_POINT_ARR       = 13; // ������������ ������ �������� NurbsPointParam
		public const int DOUBLE_ARR            = 14; // ������������ ������ duuble
		public const int CONSTRAINT_ARR        = 15; // ������������ ������ ��������������� ����������� - ��������� ConstraintParam
		public const int CORNER_ARR            = 16; // ������������ ������ �������� ���������� ����� CornerParam ��� ��������������� � ���������������
		public const int DOC_SPCOBJ_ARR        = 17; // ������������ ������ �������� ���������� ������������� ���������� � ������� ������������ DocAttachedSpcParam
		public const int SPCSUBSECTION_ARR     = 18; // ������������ ������ �������� ���������� ���������� ������������ SpcSubSectionParam
		public const int SPCTUNINGSEC_ARR      = 19; // ������������ ������ �������� ���������� ��������� ������� ������������ SpcTuningSectionParam
		public const int SPCSTYLECOLUMN_ARR    = 20; // ������������ ������ �������� ���������� ����� ������� ������� ������������ SpcStyleColumnParam
		public const int SPCSTYLESEC_ARR       = 21; // ������������ ������ �������� ���������� ����� ������a ������������ SpcStyleSectionParam
		public const int QUALITYITEM_ARR       = 22; // ������������ ������ �������� QualityItemParam - ������ �� ����� ��������� ��� ������-�� ���������
		public const int LTVARIANT_ARR         = 23; // ������������ ������ �������� LtVariant
		public const int TOLERANCEBRANCH_ARR   = 24; // ������������ ������ �������� ToleranceBranch
		public const int HATCHLINE_ARR         = 25; // ������������ ������ �������� HatchLineParam
		public const int TREENODEPARAM_ARR     = 26; // ������������ ������ �������� ���� ������ TreeNodeParam

    // ���� style ��� ������( ��������� ����� ) ��  ksTextStyleEnum:
    // 0 -������������� ����� ��� ������� ���� �������
    // 1  ����� �� �������
    // 2  ����� ��� ����������� ����������
    // 3  ����� ��������� ��������
    // 4  ����� �������������
    // 5  ����� ��� ����� �������  ( ����������� )
    // 6  ����� ��� ����� �������  ( ���\��� ������ )
    // 7  ����� ��� ����� �������  ( ����� )
    // 8  ����� ��� ������� �����
    // 9  ����� ��� ������� ( ��������� )
    // 10 ����� ��� ������� ( ������ )
    // 11 ����� ��� ����� �������
    // 12 ����� ��� ������� ����
    // 13 ����� ��� ��� ����������� �������������
    // 14 ����� ��� ����������� ���������
    // 15 ����� ��� �������� ������
    // 16 ����� ��� ������ ����
    // 17 ����� ��� �������� �������
    // 18 ����� ��� ����������� ����
    // 19 ����� ��� ����� ��������������� ���
    // 20 ����� ��� ���(�����/����������� ����������� � ������-��������)
    // 21 ����� ��� ���(�����/����������� �����������) �� �����
    // 22 ����� ��� ���(�����/����������� �����������) ��� ����� �������
    // 23 ����� ��� ���������� ������������
    // 24 ����� ��� ����� ������� ��� ����
    // 25 ����� ��� ������� ������ ( ��������� ).
    // 26 ����� ��� ������� ������ ( ������ ).

		// ��������� ��� ������ � ���������� ����������  
		public const int CHAR_ATTR_TYPE    = 1;
		public const int UCHAR_ATTR_TYPE   = 2;
		public const int INT_ATTR_TYPE     = 3;
		public const int UINT_ATTR_TYPE    = 4;
		public const int LINT_ATTR_TYPE    = 5;
		public const int FLOAT_ATTR_TYPE   = 6;
		public const int DOUBLE_ATTR_TYPE  = 7;
		public const int STRING_ATTR_TYPE  = 8;   //������ ������������� ����� MAX_TEXT_LENGTH
		public const int RECORD_ATTR_TYPE  = 9;

		// �������� ������������ ��������� �������
		public const int _AUTONOMINAL       = 0x1;   // >0 �������������� ����������� ������������ �������� �������
		public const int _RECTTEXT          = 0x2;   // >0 ����� � �������
		public const int _PREFIX            = 0x4;   // >0 ���� ����� �� ��������
		public const int _NOMINALOFF        = 0x8;   // >0 ���  ��������
		public const int _TOLERANCE         = 0x10;  // >0 �������� �����������
		public const int _DEVIATION         = 0x20;  // >0 ���������� �����������
		public const int _UNIT              = 0x40;  // >0 ������� ���������
		public const int _SUFFIX            = 0x80;  // >0 ���� ����� ����� ��������
		public const int _DEVIATION_INFORM  = 0x100; // >0 ��� ���������� _DEVIATION, ���������� ���� � ������� �������( ���� ���� �� ������ ������������ ����������).
		//    ���������� �����  ������� GetObjParam, ����� ������������ ��� �������� ����������.
		public const int _UNDER_LINE        = 0x200; // >0 ������ � ��������������
		public const int _BRACKETS          = 0x400; // >0 ������ � �������
		public const int _SQUARE_BRACKETS   = 0x800; // >0 ������ � ���������� �������, ������������ ������ � _BRACKETS
		//    _BRACKETS                    - ������ � ������� �������
		//    _BRACKETS | _SQUARE_BRACKETS - ������ � ���������� �������

		public const int   INDICATIN_TEXT_LINE_ARR        = 0xFFFF;  //��� �������������, ����������� ����� �������, ���������� � ���������
		//�������, ��� ��� ������ ������������ ������������ ������ TEXT_LINE_ARR

		// ���� ������
		public const int CURVE_STYLE    = 1;  // ����� �������
		public const int HATCH_STYLE    = 2;  // ����� ���������
		public const int TEXT_STYLE     = 3;  // ����� ������
		public const int STAMP_STYLE    = 4;  // ����� ������
		public const int CURVE_STYLE_EX = 5;  // ����� ������� �����������

		// curveType | LIKE_BASIC_LINE - ��������� ���� ��� �  �������� �����
		public const int  LIKE_BASIC_LINE = 0x10; // ��������� ���� ��� �  �������� �����
		public const int  LIKE_THIN_LINE  = 0x20; // ��������� ���� ��� �  ������ �����
		public const int  LIKE_HEAVY_LINE = 0x30; // ��������� ���� ��� �  ���������� �����

		// ����������� ��� ������� Get/SetDocOptions � ksGet/SetSysOptions
		public const int DIMENTION_OPTIONS            = 1; // ��������� �������
		public const int SNAP_OPTIONS                 = 1; // ��������� ��������
		public const int ARROWFILLING_OPTIONS         = 2; // ��������� ������� ?
		public const int SHEET_OPTIONS                = 3; // ��������� ����� ��� ����� ����������
		public const int SHEET_OPTIONS_EX             = 4; // ��������� ����� ���������
		public const int LENGTHUNITS_OPTIONS          = 5; // ��������� ������ ���������
		public const int SNAP_OPTIONS_EX              = 6; // ��������� �������� ���������
		public const int VIEWCOLOR_OPTIONS            = 7; // ��������� ����� ���� �������� ���� 2d - ����������
		public const int TEXTEDIT_VIEWCOLOR_OPTIONS   = 8; // ��������� ����� ���� �������������� ������
		public const int MODEL_VIEWCOLOR_OPTIONS      = 9; // ��������� ����� ���� ��� �������
		public const int OVERLAP_OBJECT_OPTIONS      = 10; // ��������� ��������������� ��������
		public const int DIMENTION_OPTIONS_EX        = 11; // ��������� �������

		//���� ������� ��� ������������
		public const int   SPC_CLM_FORMAT   = 1;   // ������
		public const int   SPC_CLM_ZONE     = 2;   // ����
		public const int   SPC_CLM_POS      = 3;   // �������
		public const int   SPC_CLM_MARK     = 4;   // �����������
		public const int   SPC_CLM_NAME     = 5;   // ������������
		public const int   SPC_CLM_COUNT    = 6;   // ����������
		public const int   SPC_CLM_NOTE     = 7;   // ����������
		public const int   SPC_CLM_MASSA    = 8;   // �����
		public const int   SPC_CLM_MATERIAL = 9;   // ��������
		public const int   SPC_CLM_USER     = 10;  // ����������������
		public const int   SPC_CLM_KOD      = 11;  // ���
		public const int   SPC_CLM_FACTORY  = 12;  // ����� ������������

		//���� �������� ��� ������� ������������
// �������� �� ������������ ����� ������������ LtVariantType
//		public const int   SPC_INT      = 1;   // �����
//		public const int   SPC_DOUBLE   = 2;   // ������������
//		public const int   SPC_STRING   = 3;   // ������
//		public const int   SPC_RECORD   = 4;   // ������

		//���� ������� ������
		public const int CURVE_STYLE_LIBRARY               = 1; // ���������� ������ ������ (*.lcs)
		public const int HATCH_STYLE_LIBRARY               = 2; // ���������� ������ ��������� (*.lhs)
		public const int TEXT_STYLE_LIBRARY                = 3; // ���������� ������ �������   (*.lts)
		public const int STAMP_LAYOUT_STYLE_LIBRARY        = 4; // ���������� ������ �������� ������� (*.lyt)
		public const int GRAPHIC_LAYOUT_STYLE_LIBRARY      = 5; // ���������� ������ ���������� ����������� ���������� (*.lyt)
		public const int TEXT_LAYOUT_STYLE_LIBRARY         = 6; // ���������� ������ ���������� ��������� ���������� (*.lyt)
		public const int SPC_LAYOUT_STYLE_LIBRARY          = 7; // ���������� ������ ���������� ������������ (*.lyt)

		//����������� � ���� ������ ��� �������� �����-����������� �������������
		public const int  ST_MIX_MM      = 0x1;  // ����������
		public const int  ST_MIX_SM      = 0;    // ����������
		public const int  ST_MIX_DM      = 0x2;  // ���������
		public const int  ST_MIX_M       = 0x3;  // �����
		public const int  ST_MIX_GR      = 0;    // ������
		public const int  ST_MIX_KG      = 0x10; // ����������
		public const int  ST_MIX_EXT     = 0;    // ������������
		public const int  ST_MIX_RV      = 0x20; // ��������

		// ��� ��������� ��������
		public const int  SN_NEAREST_POINT    = 1;    // ��������� �����
		public const int  SN_NEAREST_MIDDLE   = 2;    // ��������
		public const int  SN_CENTRE           = 3;    // �����
		public const int  SN_INTERSECT        = 4;    // �����������
		public const int  SN_GRID             = 5;    // �� �����
		public const int  SN_XY_ALIGN         = 6;    // ������������
		public const int  SN_ANGLE            = 7;    // ������� ��������
		public const int  SN_POINT_CURVE      = 8;    // ����� �� ������

		// ���� ����� �������� ��� ��������
		public const int  SN_DYNAMICALLY               = 0x1;  // �������� ����������� �����������
		public const int  SN_ASSISTANT                 = 0x2;  // ������ �����
		public const int  SN_BACKGROUND_LAYER          = 0x4;  // ��������� ������� ���� � ����
		public const int  SN_SUSPENDED                 = 0x8;  // �������� ��������
		public const int  SN_VISIBLE_GRID_POINTS_ONLY  = 0x10; // �������� ������ � ������� ������ �����


		// ���� ��������������� �����������
		public const int CONSTRAINT_FIXED_POINT           = 1;  // ����������� �����
		public const int CONSTRAINT_POINT_ON_CURVE        = 2;  // ����� �� ������
		public const int CONSTRAINT_HORIZONTAL            = 3;  // �����������
		public const int CONSTRAINT_VERTICAL              = 4;  // ���������
		public const int CONSTRAINT_PARALLEL              = 5;  // �������������� ���� ������ ��� ��������
		public const int CONSTRAINT_PERPENDICULAR         = 6;  // ������������������ ���� ������ ��� ��������
		public const int CONSTRAINT_EQUAL_LENGTH          = 7;  // ��������� ���� ���� ��������
		public const int CONSTRAINT_EQUAL_RADIUS          = 8;  // ��������� �������� ���� ���/�����������
		public const int CONSTRAINT_HOR_ALIGN_POINTS      = 9;  // ����������� ��� ����� �� �����������
		public const int CONSTRAINT_VER_ALIGN_POINTS      = 10; // ����������� ��� ����� �� ���������
		public const int CONSTRAINT_MERGE_POINTS          = 11; // ���������� ���� �����
		public const int CONSTRAINT_TANGENT_TWO_CURVES    = 15; // ������� ���� ������
    public const int CONSTRAINT_SYMMETRY_TWO_POINTS   = 16; // �������� ���� �����
    public const int CONSTRAINT_COLLINEAR             = 17; // ������������� ��������
    public const int CONSTRAINT_FIXED_ANGLE           = 18; // ������������� ����
    public const int CONSTRAINT_FIXED_LENGHT          = 19; // ������������� �����
    public const int CONSTRAINT_POINT_ON_CURVE_MIDDLE = 20; // ����� �� �������� ������
    public const int CONSTRAINT_BISECTOR              = 21; // �����������

		// ���� �������� ������������
		public const int  SPC_BASE_OBJECT     = 1;    // ������� ������ (������������� �������������)
		public const int  SPC_COMMENT         = 2;    // �����������    (������������� �������������)
		public const int  SPC_SECTION_NAME    = 3;    // ������������ ������� ( ��������� �� ����� �� ������������� )
		public const int  SPC_BLOCK_NAME      = 4;    // ������������ ����� ���������� ( ��������� �� ����� �� ������������� )
		public const int  SPC_RESERVE_STR     = 5;    // ��������� ������ ( ��������� �� ����� �� ������������� )
		public const int  SPC_EMPTY_STR       = 6;    // ������ ������ ( ��������� �� ����� �� ������������� )

		// ���� ����������
		public const int SPC_SORT_OFF        = 0;   // ��� ����������
		public const int SPC_SORT_COMPOS     = 1;   // ��������� ����������
		public const int SPC_SORT_ALPHABET   = 2;   // ���������� �� �������� (1.06.01 - ������ �� ������������)
		public const int SPC_SORT_UP         = 3;   // ���������� �� ����������� �������
		public const int SPC_SORT_DOCUMENT   = 4;   // ���������� ������� ������������
		public const int SPC_SORT_DOWN       = 5;   // ���������� �� �������� �������
		public const int SPC_SORT_COMPOSDOWN = 6;   // ��������� ���������� �� �������� 

		////////////////////////////////////////////////////////////////////////////////
		//
		//  ���� ����������� �������� ( ������������� ������ )
		//
		////////////////////////////////////////////////////////////////////////////////
		public const int  ARROW_INSIDE_SYMBOL       = 1;  // ������� (��������� �����) �������
		public const int  ARROW_OUT_SIDE_SYMBOL     = 2;  // ������� (��������� �����) �������
		public const int  TICK_TAIL_SYMBOL          = 3;  // ������� � ������������ ������ (� ���������)
		public const int  UP_HALF_ARROW_SYMBOL      = 4;  // ������� �������� ������� �������
		public const int  DOWN_HALF_ARROW_SYMBOL    = 5;  // ������ �������� ������� �������
		public const int  BIG_ARROW_INSIDE_SYMBOL   = 6;  // ������� ������� ������� (7��)
		public const int  ARROW_ORDINATE_DIM_SYMBOL = 7;  // ������� ��� ������� ������(������ ������ 4 �� ��� ����� 45 ��)
		public const int  TRIANGLE_SYMBOL           = 8;  // ����������� �� ����-��� ������
		public const int  CIRCLE_RAD2_SYMBOL        = 9;  // ���������� �������� 2 �� ������ ������ - ��� ���-��� � �����-�������
		public const int  CENTRE_MARKER_SYMBOL      = 10; // ����������� ���������� ������ � ���� �������� ������
		public const int  GLUE_SIGN_SYMBOL          = 11; // ���� ����������
		public const int  SOLDER_SIGN_SYMBOL        = 12; // ���� �����
		public const int  SEWING_SIGN_SYMBOL        = 13; // ���� ��������
		public const int  CRAMP_SIGN_SYMBOL         = 14; // ���� ���������� ���������� ������.�������
		public const int  CORNER_CRAMP_SIGN_SYMBOL  = 15; // ���� �������� ���������� ������.�������
		public const int  MONTAGE_JOINT_SYMBOL      = 16; // ���� ���������� ���
		public const int  TICK_SYMBOL               = 17; // ������� ��� ����������� ������ (��� ��������)
		public const int  TRIANGLE_CURR_CS          = 18; // ����������� �� ������� �� - ��� ����
		public const int  ARROW_CLOSED_INSIDE       = 19; // �������� ������� �������
		public const int  ARROW_CLOSED_OUTSIDE      = 20; // �������� ������� �������
		public const int  ARROW_OPEN_INSIDE         = 21; // �������� ������� �������
		public const int  ARROW_OPEN_OUTSIDE        = 22; // �������� ������� �������
		public const int  ARROW_RIGHTANGLE_INSIDE   = 23; // ������� 90 ���� �������
		public const int  ARROW_RIGHTANGLE_OUTSIDE  = 24; // ������� 90 ���� �������
		public const int  SYMBOL_DOT                = 25; // ����� (������� ����� ����� ������� �������)
		public const int  SYMBOL_SMALLDOT           = 26; // ����� ��������� (������� ����� 0.6 ����� ������� �������)
    public const int  AUXILIARY_POINT           = 27; // ��������������� �����
    public const int  LEFT_TICK_SYMBOL          = 28; // ������� � �������� �����
          

		//------------------------------------------------------------------------------
		// ������� ����� ��� ������� ksSetMacroParam;
		// �������� ����� ��� �������������� ������������ �����
		// ---
		public const int MP_DBL_CLICK_OFF  = 0x01; //>0 �������������� �� �������� ������� ���������
		public const int MP_HOTPOINTS      = 0x02; //>0 ��������� hot ����� �������
		public const int MP_EXTERN_EDIT    = 0x04; //>0 ��������� �������� ����������

		//-----------------------------------------------------------------------------
		//����������� ��� ����������� � ��������� ������
		// ---
		public const int FORMAT_BMP   = 0;
		public const int FORMAT_GIF   = 1;
		public const int FORMAT_JPG   = 2;
		public const int FORMAT_PNG   = 3;
		public const int FORMAT_TIF   = 4;
		public const int FORMAT_TGA   = 5;
		public const int FORMAT_PCX   = 6;
    public const int FORMAT_WMF   = 16;
    public const int FORMAT_EMF   = 17;

		//-----------------------------------------------------------------------------
		//����������� ��� ��������� ����� ���������� �������
		// ---
		public const int BLACKWHITE   = 0;   //���� ������
		public const int COLORVIEW    = 1;   //���� ������������� ��� ����
		public const int COLORLAYER   = 2;   //���� ������������� ��� ����
		public const int COLOROBJECT  = 3;   //���� ������������� ��� �������

		//-----------------------------------------------------------------------------
		// ����������� ��� �� ������ ��� ����������� � ��������� ������
		// ---
		public const int BPP_COLOR_01 = 1;  //"������" 
		public const int BPP_COLOR_02 = 2;  //"4 �����" 
		public const int BPP_COLOR_04 = 4;  //"16 ������" 
		public const int BPP_COLOR_08 = 8;  //"256 ������"
		public const int BPP_COLOR_16 = 16; //"16 ��������"
		public const int BPP_COLOR_24 = 24; //"24 �������"
		public const int BPP_COLOR_32 = 32; //"32 �������"

		//------------------------------------------------------------------------------
		// ���� ����������� �����
		// ---
		public const int VIEW_FRONT       = 0x1; //  �������
		public const int VIEW_REAR        = 0x2; //  �����
		public const int VIEW_UP          = 0x4; //  ������
		public const int VIEW_DOWN        = 0x8; //  �����
		public const int VIEW_LEFT        = 0x10; //  �����
		public const int VIEW_RIGHT       = 0x20; //  ������
		public const int VIEW_ISO         = 0x40; //  ���������

    //------------------------------------------------------------------------------
    // ���������� ������� ������
    // ---
    public const int OCR_SELECT  = 0xFFFE; //  ������ ����� SELECT 
    public const int OCR_SNAP    = 0xFFFD; //  ������ ����� SNAP 
    public const int OCR_CATCH   = 0xFFFC; //  ������ ����� CATCH
    public const int OCR_DEFAULT = 0;      //  ������ � ���� ������

    public const int OCR_DEDAULT = 0;      //  ������ � ���� ������

    //-----------------------------------------------------------------------------
    // �������������� ���� ��� TextItemFont.color
    // � ����� ����� ���� ��������� �� ��������� ���� �������� �� 0
    // � ���� ������ ���� TextItemFont.color ����� �������� 0 �� ���������
    // ����������� �� ���� � �� �� ����� ������������ ��� ���� �� ���������
    // ��� ���� ����� ����������� ����� �� �������� �����
    // ��� �������� ���� �� �������� ��� ��������� FREE_COLOR
    // ---
    public const uint FREE_COLOR     = 0xff000000; //  �������������� ����

	}
}