using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    [Serializable]
    class Unit
    {
        string unitID = "";
        public string UnitID
        {
            get
            {
                return this.unitID;
            }
        }


        public Unit(string unitID)
        {
            this.unitID = unitID;
        }

        public List<Unit> subUnit = new List<Unit>();
        public List<Item> Items = new List<Item>();

        ProcessState processState = ProcessState.IDLE;
        public ProcessState PROCESSSTATE
        {
            get
            {
                return this.processState;
            }
            set
            {
                if (this.processState == value)
                    return;
                this.processStateOld = this.processState;
                this.processState = value;
                this.processStateChanged(this, new EventArgs());
            }
        }
        public event EventHandler processStateChanged = delegate(object sender, EventArgs e) { };

        ProcessState processStateOld = ProcessState.IDLE;
        public ProcessState PROCESSSTATEOLD
        {
            get
            {
                return this.processStateOld;
            }
            set
            {
                this.processStateOld = value;
            }
        }

        EQPState eqpState = EQPState.NORMAL;
        public EQPState EQPSTATE
        {
            get
            {
                return this.eqpState;
            }
            set
            {
                if (this.eqpState == value)
                    return;
                this.eqpState = value;
                this.eqpStateChanged(this, new EventArgs());
            }
        }
        public event EventHandler eqpStateChanged = delegate(object sender, EventArgs e) { };

    }
}
