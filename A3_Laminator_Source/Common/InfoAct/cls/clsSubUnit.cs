using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsSubUnit : clsSubUnitMethod
    {
        public int UnitID = 0;
        public string ModuleID = "";                //각 Unit의 ModuleID
        public Boolean _GLSExist = false;            //GLS 존재유무      

        string _EQPState = "1";               //EQP(Module) State(1: NORMAL, 2: FAULT, 3: PM)
        public string EQPStateOLD = "1";            //EQP(Module) Old State(1: NORMAL, 2: FAULT, 3: PM)
        public string EQPState_S_OLD = "";            //EQP(Module) Old State(1: NORMAL, 2: FAULT, 3: PM)

        string _EQPProcessState = "1";        //EQP(Module) Process State(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)
        public string EQPProcessStateOLD = "1";     //EQP(Module) Old Process State(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)
        public string EQPProcessState_S_OLD = "";     //EQP(Module) Old Process State(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)

        public string EQPStateChangeBYWHO = "";             //Who Initiated the event(1: By Host, 2: By Operator, 3: By Equipment itself)
        public string EQPStateLastCommand = "";             //CIM이나 HOST로 부터 마지막으로 받은 명령

        public string EQPProcessStateChangeBYWHO = "";      //Who Initiated the event(1: By Host, 2: By Operator, 3: By Equipment itself)
        public string EQPProcessStateLastCommand = "";      //CIM이나 HOST로 부터 마지막으로 받은 명령(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)

        private string _HGLSID = "";
        private string _FilmID = "";
        private int _FilmCount = 0;
        private bool _FilmExist = false;
        private bool _FilmCaseExist = false;
        public int PortNo = 0;

        //알람 누락 수정 - 12.07.26 KSH
        public int AlarmID = 0;                     //헤비 알람 발생된 ID 저장

        public clsMultiUseData MultiData = new clsMultiUseData();

        public int DisplayLength = 0;
        public bool isElevator = false;

        //Constructor
        public clsSubUnit(int intUnitID)
        {
            this.UnitID = intUnitID;
        }

        public delegate void EQPStateChange(int UnitID, string strEqpState);
        public event EQPStateChange eqpState;
        public string EQPState
        {
            get { return _EQPState; }
            set
            {
                if (this._EQPState != value)
                {
                    this._EQPState = value;
                    if (this.eqpState != null)
                    {
                        this.eqpState(UnitID, _EQPState);
                    }
                }
            }
        }

        public delegate void EQPProcessStateChange(int UnitID, string strEQPProcessState);
        public event EQPProcessStateChange eqpProcessState;
        public string EQPProcessState
        {
            get { return _EQPProcessState; }
            set
            {
                if (_EQPProcessState != value)
                {
                    _EQPProcessState = value;
                    if (eqpProcessState != null)
                    {
                        eqpProcessState(UnitID, value);
                    }
                }
            }
        }

        public delegate void GLSExistChange(int UnitID, bool GLSExist);
        public event GLSExistChange glsexist;
        public bool GLSExist
        {
            get { return _GLSExist; }
            set
            {
                if (this._GLSExist != value)
                {
                    this._GLSExist = value;
                    if (this.glsexist != null)
                    {
                        this.glsexist(UnitID, _GLSExist);
                    }
                }
            }
        }

        public delegate void FilmCaseExistChange(int PortNo, bool FilmExist);
        public event FilmCaseExistChange filmcaseexist;
        public bool FilmCaseExist
        {
            get { return _FilmCaseExist; }
            set
            {
                if (this._FilmCaseExist != value)
                {
                    this._FilmCaseExist = value;
                    if (this.filmcaseexist != null)
                    {
                        this.filmcaseexist(PortNo, _FilmCaseExist);
                    }
                }
            }
        }


        public delegate void FilmExistChange(int UnitID, bool FilmExist);
        public event FilmExistChange filmexist;
        public bool FilmExist
        {
            get { return _FilmExist; }
            set
            {
                if (this._FilmExist != value)
                {
                    this._FilmExist = value;
                    if (this.filmexist != null)
                    {
                        this.filmexist(UnitID, _FilmExist);
                    }
                }
            }
        }

        public delegate void GlassIDChange(int unitID, string strGLSID);
        public event GlassIDChange GLSIDChange;
        public string HGLSID
        {
            get { return _HGLSID; }
            set
            {
                if (this._HGLSID != value)
                {
                    this._HGLSID = value;
                    if (GLSIDChange != null)
                    {
                        GLSIDChange(UnitID, _HGLSID);
                    }
                }
            }
        }

        public string FilmID
        {
            get { return _FilmID; }
            set
            {
                if (this._FilmID != value)
                {
                    this._FilmID = value;
                    if (GLSIDChange != null)
                    {
                        GLSIDChange(UnitID, _FilmID);
                    }
                }
            }
        }

        public int FilmCount
        {
            get { return _FilmCount; }
            set
            {
                if (this._FilmCount != value)
                {
                    this._FilmCount = value;
                    if (GLSIDChange != null)
                    {
                        GLSIDChange(UnitID, _FilmID);
                    }
                }
            }
        }

        
    }
}
