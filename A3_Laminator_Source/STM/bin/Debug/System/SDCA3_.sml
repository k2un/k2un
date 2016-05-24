<S1F0 N S1F0
>

<S2F0 N S2F0
>

<S3F0 N S3F0
>

<S5F0 N S5F0
>

<S6F0 N S6F0
>

<S7F0 N S7F0
>

<S9F0 N S9F0
>

<S10F0 N S10F0
>

<S1F1 P AreYouThere
>

<S1F2 S S1F2OnLineData
  <L 4 L1
    <A 20 VERSION >
    <A 20 SPEC_CODE >
    <A 28 MODULEID >
    <U1 1 MCMD >
  >
>

<S1F2 S S1F2IAmHere
  <L 0 L1
  >
>

<S1F3 P S1F3
  <L 2 L1
    <A 28 MODULEID >
    <L n SVCNT
      <U2 1 SVID >
    >
  >
>

<S1F4 S S1F4
  <L 2 L1
    <A 28 MODULEID >
    <L n SVCNT
      <L 5 L2
        <U2 1 SVID >
        <A 40 SV >
        <A 40 SVNAME >
        <A 10 DATATYPE >
        <A 28 MODULEID1 >
      >
    >
  >
>

<S1F5 P S1F5
  <L 2 L1
    <A 28 MODULEID >
    <U1 1 SFCD >
  >
>

<S1F6 S S1F6WrongMODID
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 SFCD >
    <L 0 L2
    >
  >
>

<S1F6 S S1F6SFCD1
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 SFCD >
    <L n EOIDCNT
      <L 2 L2
        <U1 1 EOID >
        <L n EOMDCNT
          <L 2 L3
            <U1 1 EOMD >
            <U1 1 EOV >
          >
        >
      >
    >
  >
>

<S1F6 S S1F6SFCD2
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 SFCD >
    <L n PORTCNT
      <L 2 L2
        <L 5 L3_1
          <A 4 PORTID >
          <U1 1 PTSTATE >
          <U1 1 PTTYPE >
          <A 2 PTMODE >
          <U1 1 SORTTYPE >
        >
        <L 5 L3_2
          <A 12 CSTID >
          <U1 1 CSTTYPE >
          <A 28 MAPSTIF >
          <A 28 CURSTIF >
          <U1 1 BATORDER >
        >
      >
    >
  >
>

<S1F6 S S1F6SFCD3
  <L n UNITCNT
    <L 3 L1
      <A 28 MODULEID >
      <U1 1 SFCD >
      <L n GLSCNT
        <L 21 L2
          <A 16 H_PANELID >
          <A 16 E_PANELID >
          <A 16 LOTID >
          <A 16 BATCHID >
          <A 16 JOBID >
          <A 4 PORTID >
          <A 2 SLOTNO >
          <A 4 PROD_TYPE >
          <A 4 PROD_KIND >
          <A 16 PRODID >
          <A 16 RUNSPECID >
          <A 8 LAYERID >
          <A 8 STEPID >
          <A 20 PPID >
          <A 20 FLOWID >
          <U2 2 SIZE >
          <U2 1 THICKNESS >
          <U2 1 STATE >
          <A 4 ORDER >
          <A 16 COMMENT >
          <L 3 L3
            <L 10 L4_1
              <A 4 USE_CNT >
              <A 4 JUDGE >
              <A 4 REASONCODE >
              <A 2 INS_FLAG >
              <A 2 ENC_FLAG >
              <A 2 PRERUNFLAG >
              <A 2 TURN_DIR >
              <A 2 FLIPSTATE >
              <A 4 WORKSTATE >
              <A 16 MULTIUSE >
            >
            <L 2 L4_2
              <A 16 PAIR_GLSID >
              <A 20 PAIR_PPID >
            >
            <L 5 L4_3
              <L 2 L5_1
                <A 40 OPT_NAME1 >
                <A 40 OPT_VALUE1 >
              >
              <L 2 L5_2
                <A 40 OPT_NAME2 >
                <A 40 OPT_VALUE2 >
              >
              <L 2 L5_3
                <A 40 OPT_NAME3 >
                <A 40 OPT_VALUE3 >
              >
              <L 2 L5_4
                <A 40 OPT_NAME4 >
                <A 40 OPT_VALUE4 >
              >
              <L 2 L5_5
                <A 40 OPT_NAME5 >
                <A 40 OPT_VALUE5 >
              >
            >
          >
        >
      >
    >
  >
>

<S1F6 S S1F6SFCD4
  <L 2 L1
    <U1 1 SFCD >
    <L 5 L2
      <A 28 MODULEID >
      <U1 1 MODULE_STATE >
      <U1 1 PROC_STATE >
      <U1 1 MCMD >
      <L n LAYER1CNT
        <L 4 L3
          <A 28 MODULEID1 >
          <U1 1 MODULE_STATE1 >
          <U1 1 PROC_STATE1 >
          <L n LAYER2CNT
            <L 4 L4
              <A 28 MODULEID2 >
              <U1 1 MODULE_STATE2 >
              <U1 1 PROC_STATE2 >
              <L n LAYER3CNT
                <L 4 L5
                  <A 28 MODULEID3 >
                  <U1 1 MODULE_STATE3 >
                  <U1 1 PROC_STATE3 >
                  <L n LAYER4CNT
                    <L 3 L6
                      <A 28 MODULEID4 >
                      <U1 1 MODULE_STATE4 >
                      <U1 1 PROC_STATE4 >
                    >
                  >
                >
              >
            >
          >
        >
      >
    >
  >
>

<S1F6 S S1F6SFCD31
  <L 2 L1
    <U1 1 SFCD >
    <L 7 L2
      <A 28 MODULEID >
      <U1 1 MODULE_STATE >
      <U1 1 PROC_STATE >
      <U1 1 CUR_STEPNO >
      <L n CUR_STEPCNT
        <L 2 L3
          <U1 1 STEPNO >
          <A 40 STEP_DESC >
        >
      >
      <L 2 L2_1
        <L n CUR_GLSCNT
          <A 16 H_GLASSID >
        >
        <L n CUR_M_CNT
          <A 16 MATERIAL_ID >
        >
      >
      <L n LAYER1CNT
        <L 7 L4
          <A 28 MODULEID1 >
          <U1 1 MODULE_STATE1 >
          <U1 1 PROC_STATE1 >
          <U1 1 CUR_STEPNO1 >
          <L n CUR_STEPCNT1
            <L 2 L5
              <U1 1 STEPNO1 >
              <A 40 STEP_DESC1 >
            >
          >
          <L 2 L4_1
            <L n CUR_GLSCNT1
              <A 16 H_GLASSID1 >
            >
            <L n CUR_M_CNT1
              <A 16 MATERIAL_ID1 >
            >
          >
          <L n LAYER2CNT
            <L 7 L6
              <A 28 MODULEID2 >
              <U1 1 MODULE_STATE2 >
              <U1 1 PROC_STATE2 >
              <U1 1 CUR_STEPNO2 >
              <L n CUR_STEPCNT2
                <L 2 L7
                  <U1 1 STEPNO2 >
                  <A 40 STEP_DESC2 >
                >
              >
              <L 2 L6_1
                <L n CUR_GLSCNT2
                  <A 16 H_GLASSID2 >
                >
                <L n CUR_M_CNT2
                  <A 16 MATERIAL_ID2 >
                >
              >
              <L n LAYER3CNT
                <L 7 L8
                  <A 28 MODULEID3 >
                  <U1 1 MODULE_STATE3 >
                  <U1 1 PROC_STATE3 >
                  <U1 1 CUR_STEPNO3 >
                  <L n CUR_STEPCNT3
                    <L 2 L9
                      <U1 1 STEPNO3 >
                      <A 40 STEP_DESC3 >
                    >
                  >
                  <L 2 L8_1
                    <L n CUR_GLSCNT3
                      <A 16 H_GLASSID3 >
                    >
                    <L n CUR_M_CNT3
                      <A 16 MATERIAL_ID3 >
                    >
                  >
                  <L n LAYER4CNT
                    <L 6 La
                      <A 28 MODULEID4 >
                      <U1 1 MODULE_STATE4 >
                      <U1 1 PROC_STATE4 >
                      <U1 1 CUR_STEPNO4 >
                      <L n CUR_STEPCNT4
                        <L 2 Lb
                          <U1 1 STEPNO4 >
                          <A 40 STEP_DESC4 >
                        >
                      >
                      <L 2 La_1
                        <L n CUR_GLSCNT4
                          <A 16 H_GLASSID4 >
                        >
                        <L n CUR_M_CNT4
                          <A 16 MATERIAL_ID4 >
                        >
                      >
                    >
                  >
                >
              >
            >
          >
        >
      >
    >
  >
>

<S1F17 P S1F17
  <L 2 L1
    <A 28 MODULEID >
    <U1 1 MCMD >
  >
>

<S1F18 S S1F18
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 MCMD >
    <U1 1 ONLACK >
  >
>

<S2F15 P S2F15
  <L 2 L1
    <A 28 MODULEID >
    <L n ECCOUNT
      <L 7 L2
        <U2 1 ECID >
        <A 40 ECNAME >
        <A 40 ECDEF >
        <A 40 ECSLL >
        <A 40 ECSUL >
        <A 40 ECWLL >
        <A 40 ECWUL >
      >
    >
  >
>

<S2F16 S S2F16
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 MIACK >
    <L n ECCOUNT
      <L 8 L2
        <U1 1 TEAC >
        <L 2 L2_1
          <U2 1 ECID >
          <U1 1 EAC1 >
        >
        <L 2 L2_2
          <A 40 ECNAME >
          <U1 1 EAC2 >
        >
        <L 2 L2_3
          <A 40 ECDEF >
          <U1 1 EAC3 >
        >
        <L 2 L2_4
          <A 40 ECSLL >
          <U1 1 EAC4 >
        >
        <L 2 L2_5
          <A 40 ECSUL >
          <U1 1 EAC5 >
        >
        <L 2 L2_6
          <A 40 ECWLL >
          <U1 1 EAC6 >
        >
        <L 2 L2_7
          <A 40 ECWUL >
          <U1 1 EAC7 >
        >
      >
    >
  >
>

<S2F23 P S2F23
  <L 6 L1
    <A 28 MODULEID >
    <U2 1 TRID >
    <A 6 SMPTIME >
    <U2 1 TOTSMP >
    <U2 1 GRSIZE >
    <L n SVCOUNT
      <U2 1 SVID >
    >
  >
>

<S2F24 S S2F24
  <U1 1 TIACK >
>

<S2F29 P S2F29
  <L 2 L1
    <A 28 MODULEID >
    <L n ECIDCNT
      <U2 1 ECID >
    >
  >
>

<S2F30 S S2F30
  <L 2 L1
    <A 28 MODULEID >
    <L n ECCOUNT
      <L 7 L2
        <U2 1 ECID >
        <A 40 ECNAME >
        <A 40 ECDEF >
        <A 40 ECSLL >
        <A 40 ECSUL >
        <A 40 ECWLL >
        <A 40 ECWUL >
      >
    >
  >
>

<S2F31 P S2F31
  <A 14 TIME >
>

<S2F32 S S2F32
  <U1 1 ACKC2 >
>

<S2F41 P S2F41EQPCMD
  <L 2 L1
    <U2 1 RCMD >
    <L n MODULECOUNT
      <L 2 L2
        <A 10 MODULEID1 >
        <A 28 MODULEID >
      >
    >
  >
>

<S2F42 S S2F42EQPCMDREPLY
  <L 3 L1
    <U2 1 RCMD >
    <U1 1 HCACK >
    <L n MODULECOUNT
      <L 3 L2
        <A 10 MODULEID1 >
        <A 28 MODULEID >
        <U1 1 CPACK >
      >
    >
  >
>

<S2F101 P S2F101
  <L 3 L1
    <U1 1 TID >
    <A 28 MODULEID >
    <L n MSGCNT
      <A 100 MSG >
    >
  >
>

<S2F102 S S2F102
  <U1 1 ACKC2 >
>

<S2F103 P S2F103
  <L 2 L1
    <A 28 MODULEID >
    <L n EOIDCOUNT
      <L 2 L2
        <U1 1 EOID >
        <L n EOMDCOUNT
          <L 2 L3
            <U1 1 EOMD >
            <U1 1 EOV >
          >
        >
      >
    >
  >
>

<S2F104 S S2F104
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 MIACK >
    <L n EOIDCOUNT
      <L 2 L2
        <U1 1 EOID >
        <L n EOMDCOUNT
          <L 2 L3
            <U1 1 EOMD >
            <U1 1 EAC >
          >
        >
      >
    >
  >
>

<S3F1 P S3F1
  <A 28 MODULEID >
>

<S3F2 S S3F2
  <L 2 L1
    <A 28 MODULEID >
    <L n M_COUNT
      <L 8 L2
        <A 16 M_ID >
        <A 4 PROD_TYPE >
        <A 16 LIBRARYID >
        <A 16 STAGE_STATE >
        <U2 1 M_STATE >
        <A 20 LOCATION >
        <U2 1 M_SIZE >
        <L n PRODCNT
          <L 3 L3
            <A 16 PROD_ID >
            <A 8 STEPID >
            <A 20 PPID >
          >
        >
      >
    >
  >
>

<S3F115 P S3F115MaterialInfo
  <L 3 L1
    <U1 1 HOT_DEVICE >
    <U1 1 HOT_LEVEL >
    <L 3 L2
      <L 5 L2_1
        <A 4 PORTID >
        <U1 1 PORT_STATE >
        <U1 1 PORT_TYPE >
        <A 2 PORT_MODE >
        <U1 1 SORT_TYPE >
      >
      <L 5 L2_2
        <A 12 CSTID >
        <U1 1 CST_TYPE >
        <A 28 MAP_STIF >
        <A 28 CUR_STIF >
        <U1 1 BATCH_ORDER >
      >
      <L n M_COUNT
        <L 22 L3
          <A 16 MATERIALID >
          <A 16 M_SETID >
          <A 16 LOTID >
          <A 16 BATCHID >
          <A 16 JOBID >
          <A 4 PORTID1 >
          <A 2 SLOTNO >
          <A 4 PROD_TYPE >
          <A 4 PROD_KIND >
          <A 16 PRODUCTID >
          <A 16 RUNSPECID >
          <A 8 LAYERID >
          <A 8 STEPID >
          <A 20 PPID >
          <A 20 FLOWID >
          <U2 2 M_SIZE >
          <U2 1 M_THICKNESS >
          <U2 1 M_STATE >
          <A 4 M_ORDER >
          <A 16 COMMENT >
          <L 3 L4
            <L 10 L4_1
              <A 4 USE_COUNT >
              <A 4 JUDGEMENT >
              <A 4 REASON_CODE >
              <A 2 INS_FLAG >
              <A 2 LIBRARYID >
              <A 2 PRERUN_FLAG >
              <A 2 TURN_DIR >
              <A 2 FLIP_STATE >
              <A 4 WORK_STATE >
              <A 16 MULTI_USE >
            >
            <L 2 L4_2
              <A 16 STAGE_STATE >
              <A 20 LOCATION >
            >
            <L 5 L4_3
              <L 2 L5_1
                <A 40 OPTION_NAME1 >
                <A 40 OPTION_VALUE1 >
              >
              <L 2 L5_2
                <A 40 OPTION_NAME2 >
                <A 40 OPTION_VALUE2 >
              >
              <L 2 L5_3
                <A 40 OPTION_NAME3 >
                <A 40 OPTION_VALUE3 >
              >
              <L 2 L5_4
                <A 40 OPTION_NAME4 >
                <A 40 OPTION_VALUE4 >
              >
              <L 2 L5_5
                <A 40 OPTION_NAME5 >
                <A 40 OPTION_VALUE5 >
              >
            >
          >
          <L n SUBMATERIALCOUNT
            <L 15 L6
              <A 30 SUB_MID >
              <A 8 SUB_MKIND >
              <A 8 SUB_MTYPE >
              <A 4 SUB_MMODEL >
              <A 10 SUB_MMAKER >
              <A 10 SUB_MMATTER >
              <U2 1 SUB_MTHICK >
              <U2 1 SUB_MSTATE >
              <A 4 SUB_POSITION >
              <A 4 SUB_LAYER >
              <A 14 SUB_DATE_FABIN >
              <A 14 SUB_DATE_DISCARD >
              <A 14 SUB_DATE_ETCH >
              <A 14 SUB_DATE_SHIP >
              <A 4 SUB_JUDGE >
            >
          >
        >
      >
    >
  >
>

<S3F116 S S3F116MaterialInfoReply
  <L 1 L1
    <U1 1 ACKC3 >
  >
>

<S5F1 P S5F1
  <L 2 L1
    <U1 1 SETCODE >
    <L 4 AlarmItems
      <A 28 MODULEID >
      <U1 1 ALCD >
      <U4 1 ALID >
      <A 100 ALTX >
    >
  >
>

<S5F5 P S5F5
  <L 2 L1
    <A 28 MODULEID >
    <L n ALARMCOUNT
      <U4 1 ALID >
    >
  >
>

<S5F2 S S5F2
  <U1 1 ACKC5 >
>

<S5F6 S S5F6
  <L 2 L1
    <A 28 MODULEID >
    <L n ALARMCOUNT
      <L 4 L2
        <A 28 MODULEID1 >
        <U1 1 ALCD >
        <U4 1 ALID >
        <A 100 ALTX >
      >
    >
  >
>

<S5F101 P S5F101
  <A 28 MODULEID >
>

<S5F102 S S5F102
  <L n ALARMCOUNT
    <L 4 L1
      <A 28 MODULEID >
      <U1 1 ALCD >
      <U4 1 ALID >
      <A 100 ALTX >
    >
  >
>

<S6F1 N S6F1
  <L 2 L1
    <A 28 MODULEID >
    <L 3 L2
      <U2 1 TRID >
      <U2 1 SMPLN >
      <L n REPGSZCOUNTER
        <L 2 L3
          <A 16 SCTIME >
          <L n SVCOUNT
            <L 3 L4
              <U2 1 SVID >
              <A 40 SV >
              <A 40 SVNAME >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedPort
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L 5 L3_2
          <A 4 PORTID >
          <U1 1 PORT_STATE >
          <U1 1 PORT_TYPE >
          <A 2 PORT_MODE >
          <U1 1 SORT_TYPE >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L 5 L3_3
          <A 12 CSTID >
          <U1 1 CST_TYPE >
          <A 56 MAP_STIF >
          <A 56 CUR_STIF >
          <U1 1 BATCH_ORDER >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedEQPEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L 2 L3_2
          <U1 1 OLD_STATE >
          <U1 1 NEW_STATE >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L 4 L3_3
          <A 28 MODULEID1 >
          <U1 1 ALCD >
          <U4 1 ALID >
          <A 100 ALTX >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedEQPParameter
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n EOCOUNT
          <L 2 L3_2
            <U1 1 EOID >
            <L n EOMDCOUNT
              <L 2 L4
                <U1 1 EOMD >
                <U1 1 EOV >
              >
            >
          >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L n ECCOUNT
          <L 7 L3_3
            <U2 1 ECID >
            <A 40 ECNAME >
            <A 40 ECDEF >
            <A 40 ECSLL >
            <A 40 ECSUL >
            <A 40 ECWLL >
            <A 40 ECWUL >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11EQPSpecifiedCtrl
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n COUNT
          <L 2 L3_2
            <A 20 ITEM_NAME >
            <A 20 ITEM_VALUE >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedMaterialEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n GLASSCOUNT
          <L 22 L3_2
            <A 16 MATERIAL_ID >
            <A 16 MATERIAL_SETID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID1 >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 MATERIAL_SIZE >
            <U2 1 MATERIAL_THICKNESS >
            <U2 1 MATERIAL_STATE >
            <A 4 MATERIAL_ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <A 2 INS_FLAG >
                <A 2 ENC_FLAG >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 STAGE_STATE >
                <A 20 LOCATION >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
            <L n L6
              <L 15 L6_L1
                <A 30 SUB_MATERIALID >
                <A 8 SUB_MATERIAL_KIND >
                <A 8 SUB_MATERIAL_TYPE >
                <A 4 SUB_MATERIAL_MODEL >
                <A 10 SUB_MATERIAL_MAKER >
                <A 10 SUB_MATERIAL_MATTER >
                <U2 1 SUB_MATERIAL_THICKNESS >
                <U2 1 SUB_MATERIAL_STATE >
                <A 4 SUB_POSITION >
                <A 4 SUB_LAYER >
                <A 14 SUB_DATE_FABIN >
                <A 14 SUB_DATE_DISCARD >
                <A 14 SUB_DATE_ETCHING >
                <A 14 SUB_DATE_SHIPPING >
                <A 4 SUB_JUDGEMENT >
              >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11MaterialStockEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3_1
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <A 28 MODULEID2 >
        <L n COUNT
          <L 8 L3_2
            <A 16 MATERIAL_ID >
            <A 4 PROD_TYPE >
            <A 8 LIBRARYID >
            <A 16 STAGE_STATE >
            <U2 1 MATERIAL_STATE >
            <A 20 LOCATION >
            <U2 2 MATERIAL_SIZE >
            <L n L4
              <L 3 L4_1
                <A 16 PRODUCTID >
                <A 8 STEPID >
                <A 20 PPID2 >
              >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedProcessStatusEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 4 L2_1
        <A 28 MODULEID >
        <U1 1 MCMD >
        <U1 1 MODULE_STATE >
        <U1 1 PROC_STATE >
      >
      <L 2 L2_2
        <U1 1 STEPNO >
        <U1 1 PREV_STEPNO >
      >
      <L n L2_3
        <L 4 L4
          <A 40 IDENTIFICATION >
          <U1 1 POSITION >
          <A 1 PROCESS_ACT >
          <A 20 PROCESSED_PROD >
        >
      >
    >
  >
>

<S6F12 S S6F12
  <U1 1 ACKC6 >
>

<S6F13 P S6F13GLSAPD
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L n GLASSCOUNT
          <L 21 L3
            <A 16 H_GLASSID >
            <A 16 E_GLASSID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <A 2 INS_FLAG >
                <A 2 ENC_FLAG >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 PAIR_GLASSID >
                <A 20 PAIR_PPID >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
          >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n MODULECNT
          <L 2 L3_2
            <A 28 MODULEID >
            <L n DATACOUNT
              <L 2 L4_4
                <A 40 DATA_ITEM >
                <A 40 DATA_VALUE >
              >
            >
          >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L n GLSCNT
          <L 2 L3_3
            <A 40 FLOW_PATH >
            <A 40 FLOW_MODULEID >
          >
        >
      >
    >
  >
>

<S6F13 P S6F13LOTAPD
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L n GLASSCOUNT
          <L 21 L3
            <A 16 H_GLASSID >
            <A 16 E_GLASSID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 0 L4_1
              >
              <L 0 L4_2
              >
              <L 0 L4_3
              >
            >
          >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n MODULECNT
          <L 2 L3_1
            <A 28 MODULEID >
            <L n DATACOUNT
              <L 2 L4_4
                <A 40 DATA_ITEM >
                <A 40 DATA_VALUE >
              >
            >
          >
        >
      >
    >
  >
>

<S6F13 P S6F13GLSAPDMea
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L n GLASSCOUNT
          <L 21 L3
            <A 16 H_GLASSID >
            <A 16 E_GLASSID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID >
            <A 2 SLOTNO >
            <A 4 PRODUCT_TYPE >
            <A 4 PRODUCT_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <U2 1 INS_FLAG >
                <A 2 ENC_FLAG >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 PAIR_GLASSID >
                <A 20 PAIR_PPID >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
          >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n MODULECNT
          <L 2 L3_1
            <A 28 MODULEID >
            <L 4 L4_4
              <L 2 L4_5
                <A 16 RAWPATH >
                <A 100 RAWPATH_Value >
              >
              <L 2 L4_6
                <A 16 SUMPATH >
                <A 100 SUMPATH_VALUE >
              >
              <L 2 L4_7
                <A 16 IMGPATH >
                <A 100 IMGPATH_VALUE >
              >
              <L 2 L4_8
                <A 16 DISK >
                <A 100 DISK_VALUE >
              >
            >
          >
        >
      >
    >
  >
>

<S6F14 S S6F14
  <U1 1 ACKC6 >
>

<S7F1 P S7F1
  <L 3 L1
    <A 28 MODULEID >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
  >
>

<S7F2 S S7F2
  <U1 1 PPGNT >
>

<S7F23 P S7F23
  <L 4 L1
    <A 28 MODULEID >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
    <L n COMMANDCOUNT
      <L 2 L2
        <U2 1 CCODE >
        <L n PPARMCOUNT
          <L 2 L3
            <A 40 P_PARM_NAME >
            <A 40 P_PARM >
          >
        >
      >
    >
  >
>

<S7F24 S S7F24
  <U1 1 ACKC7 >
>

<S7F25 P S7F25
  <L 3 L1
    <A 28 MODULEID >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
  >
>

<S7F26 S S7F26
  <L 4 L1
    <A 28 MODULEID >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
    <L n COMMANDCOUNT
      <L 2 L2
        <U2 1 CCODE >
        <L n PPARMCOUNT
          <L 2 L3
            <A 40 P_PARM_NAME >
            <A 40 P_PARM >
          >
        >
      >
    >
  >
>

<S7F101 P S7F101
  <L 2 L1
    <A 28 MODULEID >
    <U1 1 PPID_TYPE >
  >
>

<S7F102 S S7F102
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 PPID_TYPE >
    <L n PPIDCOUNT
      <A 20 PPID >
    >
  >
>

<S7F103 P S7F103
  <L 3 L1
    <A 28 MODULEID >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
  >
>

<S7F104 S S7F104
  <U1 1 ACKC7 >
>

<S7F107 P S7F107
  <L 5 L1
    <U1 1 MODE >
    <A 28 MODULEID1 >
    <A 20 PPID >
    <U1 1 PPID_TYPE >
    <L n COMMANDCOUNT
      <L 2 L2
        <U2 1 CCODE >
        <L n PPARMCOUNT
          <L 2 L3
            <A 40 P_PARM_NAME >
            <A 40 P_PARM >
          >
        >
      >
    >
  >
>

<S7F108 S S7F108
  <U1 1 ACK7 >
>

<S7F109 P S7F109
  <L 2 L1
    <A 28 MODULEID >
    <U1 1 PPID_TYPE >
  >
>

<S7F110 S S7F110
  <L 2 L1
    <U1 1 ACKC7 >
    <L 3 L2
      <A 28 MODULEID >
      <A 20 PPID >
      <U1 1 PPID_TYPE >
    >
  >
>

<S9F1 N S9F1
  <BINARY 10 MHEAD >
>

<S9F3 N S9F3
  <BINARY 10 MHEAD >
>

<S9F5 N S9F5
  <BINARY 10 MHEAD >
>

<S9F7 N S9F7
  <BINARY 10 MHEAD >
>

<S9F9 N S9F9
  <BINARY 10 MHEAD >
>

<S9F11 N S9F11
  <BINARY 10 MHEAD >
>

<S10F3 N S10F3
  <L 3 L1
    <U1 1 TID >
    <A 28 MODULEID >
    <L n MSGCNT
      <A 100 MSG >
    >
  >
>

<S2F41 P S2F41GLSCMD
  <L 2 L1
    <U2 1 RCMD >
    <L 3 MODULECOUNT
      <L 2 L2_1
        <A 10 "MODULEID  " >
        <A 28 MODULEID >
      >
      <L 2 L2_2
        <A 10 "H_GLASSID " >
        <A 16 H_GLASSID >
      >
      <L 2 L2_3
        <A 10 "SLOTNO    " >
        <L n SLOTNO
          <U1 1 SLOT_NO1 >
        >
      >
    >
  >
>

<S2F42 S S2F42GLSCMDREPLY
  <L 3 L1
    <U2 1 RCMD >
    <U1 1 HCACK >
    <L 3 MODULECOUNT
      <L 3 L2_1
        <A 10 "MODULEID  " >
        <A 28 MODULEID >
        <U1 1 CPACK >
      >
      <L 2 L1_2
        <A 10 "H_GLASSID " >
        <A 16 H_GLASSID >
        <U1 1 CPACK1 >
      >
      <L 3 L2_5
        <A 10 "SLOTNO    " >
        <L n SLOTCNT
          <U1 1 SLOT_NO >
        >
        <U1 1 CPACK2 >
      >
    >
  >
>

<S2F41 P S2F41MATERIALCMD
  <L 2 L1
    <U2 1 RCMD >
    <L 3 MODULECOUNT
      <L 2 L2_1
        <A 10 "MODULEID  " >
        <A 28 MODULEID >
      >
      <L 2 L2_2
        <A 10 "MATERIALID" >
        <A 16 MATERIALID >
      >
      <L 2 L2_3
        <A 10 "SLOTNO    " >
        <L n SLOTNO
          <U1 1 SLOT_NO1 >
        >
      >
    >
  >
>

<S2F42 S S2F42M_CMDREPLY
  <L 3 L1
    <U2 1 RCMD >
    <U1 1 HCACK >
    <L 3 MODULECOUNT
      <L 3 L2_1
        <A 10 "MODULEID  " >
        <A 28 MODULEID >
        <U1 1 CPACK >
      >
      <L 2 L1_2
        <A 10 "MATERIALID" >
        <A 16 MATERIALID >
        <U1 1 CPACK1 >
      >
      <L 3 L2_3
        <A 10 "SLOTNO    " >
        <L n SLOTCNT
          <U1 1 SLOT_NO >
        >
        <U1 1 CPACK2 >
      >
    >
  >
>

<S3F101 P S3F101CSTINFO
  <L 3 L1
    <U1 1 HOT_DEVICE >
    <U1 1 HOT_LEVEL >
    <L 3 L2
      <L 5 L2_1
        <A 4 PORTID >
        <U1 1 PORT_STATE >
        <U1 1 PORT_TYPE >
        <A 2 PORT_MODE >
        <U1 1 SORT_TYPE >
      >
      <L 5 L2_2
        <A 12 CSTID >
        <U1 1 CST_TYPE >
        <A 28 MAP_STIF >
        <A 28 CUR_STIF >
        <U1 1 BATCH_ORDER >
      >
      <L n GLASSCOUNT
        <L 21 L3
          <A 16 H_GLASSID >
          <A 16 E_GLASSID >
          <A 16 LOTID >
          <A 16 BATCHID >
          <A 16 JOBID >
          <A 4 PORTID1 >
          <A 2 SLOTNO >
          <A 4 PROD_TYPE >
          <A 4 PROD_KIND >
          <A 16 PRODUCTID >
          <A 16 RUNSPECID >
          <A 8 LAYERID >
          <A 8 STEPID >
          <A 20 PPID >
          <A 20 FLOWID >
          <U2 2 SIZE >
          <U2 1 THICKNESS >
          <U2 1 STATE >
          <A 4 ORDER >
          <A 16 COMMENT >
          <L 3 L4
            <L 10 L4_1
              <A 4 USE_COUNT >
              <A 4 JUDGEMENT >
              <A 4 REASON_CODE >
              <U2 1 INS_BIT_FLAG >
              <A 2 INS_FLAG2 >
              <A 2 PRERUN_FLAG >
              <A 2 TURN_DIR >
              <A 2 FLIP_STATE >
              <A 4 WORK_STATE >
              <A 16 MULTI_USE >
            >
            <L 2 L4_2
              <A 16 PARENT_GLSID >
              <A 20 RESERVED >
            >
            <L 5 L4_3
              <L 2 L5_1
                <A 40 OPTION_NAME1 >
                <A 40 OPTION_VALUE1 >
              >
              <L 2 L5_2
                <A 40 OPTION_NAME2 >
                <A 40 OPTION_VALUE2 >
              >
              <L 2 L5_3
                <A 40 OPTION_NAME3 >
                <A 40 OPTION_VALUE3 >
              >
              <L 2 L5_4
                <A 40 OPTION_NAME4 >
                <A 40 OPTION_VALUE4 >
              >
              <L 2 L5_5
                <A 40 OPTION_NAME5 >
                <A 40 OPTION_VALUE5 >
              >
            >
          >
        >
      >
    >
  >
>

<S3F102 S S3F103CSTINFOReply
  <L 1 L1
    <U1 1 ACKC3 >
  >
>

<S6F11 P S6F11RelatedJobProcess
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 4 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L 5 L3_1
          <A 4 PORTID >
          <U1 1 PORT_STATE >
          <U1 1 PORT_TYPE >
          <A 2 PORT_MODE >
          <U1 1 SORT_TYPE >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L 5 L3_2
          <A 12 CSTID >
          <U1 1 CST_TYPE >
          <A 56 MAP_STIF >
          <A 56 CUR_STIF >
          <U1 1 BATCH_ORDER >
        >
      >
      <L 2 L2_4
        <U1 1 RPTID3 >
        <L n GLASSCOUNT
          <L 21 L3_3
            <A 16 H_GLASSID >
            <A 16 E_GLASSID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID1 >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <U2 1 INS_BIT_FLAG >
                <A 2 INS_FLAF2 >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 PAIR_GLASSID >
                <A 20 PAIR_PPID >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11RelatedGlassProcess
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_4
        <U1 1 RPTID2 >
        <L n GLASSCOUNT
          <L 21 L3_1
            <A 16 H_GLASSID >
            <A 16 E_GLASSID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID1 >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <A 2 INS_FLAG >
                <A 2 ENC_FLAG >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 PAIR_GLASSID >
                <A 20 PAIR_PPID >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11MaterialProcEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID2 >
        <L n GLASSCOUNT
          <L 22 L3_1
            <A 16 MATERIAL_ID >
            <A 16 MATERIAL_SETID >
            <A 16 LOTID >
            <A 16 BATCHID >
            <A 16 JOBID >
            <A 4 PORTID1 >
            <A 2 SLOTNO >
            <A 4 PROD_TYPE >
            <A 4 PROD_KIND >
            <A 16 PRODUCTID >
            <A 16 RUNSPECID >
            <A 8 LAYERID >
            <A 8 STEPID >
            <A 20 PPID >
            <A 20 FLOWID >
            <U2 2 SIZE >
            <U2 1 THICKNESS >
            <U2 1 STATE >
            <A 4 ORDER >
            <A 16 COMMENT >
            <L 3 L4
              <L 10 L4_1
                <A 4 USE_COUNT >
                <A 4 JUDGEMENT >
                <A 4 REASON_CODE >
                <A 2 INS_FLAG >
                <A 2 ENC_FLAG >
                <A 2 PRERUN_FLAG >
                <A 2 TURN_DIR >
                <A 2 FLIP_STATE >
                <A 4 WORK_STATE >
                <A 16 MULTI_USE >
              >
              <L 2 L4_2
                <A 16 STAGE_STATE >
                <A 20 LOCATION >
              >
              <L 5 L4_3
                <L 2 L5_1
                  <A 40 OPTION_NAME1 >
                  <A 40 OPTION_VALUE1 >
                >
                <L 2 L5_2
                  <A 40 OPTION_NAME2 >
                  <A 40 OPTION_VALUE2 >
                >
                <L 2 L5_3
                  <A 40 OPTION_NAME3 >
                  <A 40 OPTION_VALUE3 >
                >
                <L 2 L5_4
                  <A 40 OPTION_NAME4 >
                  <A 40 OPTION_VALUE4 >
                >
                <L 2 L5_5
                  <A 40 OPTION_NAME5 >
                  <A 40 OPTION_VALUE5 >
                >
              >
            >
            <L 1 L4_4
              <L 15 Sub
                <A 30 SUB_MATERIALID >
                <A 8 SUB_MATERIAL_KIND >
                <A 8 SUB_MATERIAL_TYPE >
                <A 4 SUB_MATERIAL_MODEL >
                <A 10 SUB_MATERIAL_MAKER >
                <A 10 SUB_MATERIAL_MATTER >
                <U2 1 SUB_MATERIAL_THICKNESS >
                <U2 1 SUB_MATERIAL_STATE >
                <A 4 SUB_POSITION >
                <A 4 SUB_LAYER >
                <A 14 SUB_DATE_FABIN >
                <A 14 SUB_DATE_DISCARD >
                <A 14 SUB_DATE_ETCHING >
                <A 14 SUB_DATE_SHIPPING >
                <A 4 SUB_JUDGEMENT >
              >
            >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11EQPSpecifiedNetwork
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 2 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L n COUNT
          <L 2 L3_1
            <A 40 ITEM_NAME >
            <A 40 ITEM_VALUE >
          >
        >
      >
    >
  >
>

<S6F11 P S6F11MaterialLoadEvent
  <L 3 L1
    <U1 1 DATAID >
    <U2 1 CEID >
    <L 3 L2
      <L 2 L2_1
        <U1 1 RPTID >
        <L 6 L3
          <A 28 MODULEID >
          <U1 1 MCMD >
          <U1 1 MODULE_STATE >
          <U1 1 PROC_STATE >
          <U1 1 BYWHO >
          <A 16 OPERID >
        >
      >
      <L 2 L2_2
        <U1 1 RPTID1 >
        <L 5 L2_2_1
          <A 4 PORTID >
          <U1 1 PORT_STATE >
          <U1 1 PORT_TYPE >
          <A 2 PORT_MODE >
          <U1 1 SORT_TYPE >
        >
      >
      <L 2 L2_3
        <U1 1 RPTID2 >
        <L n MATERIALCOUNT
          <L 3 L2_3_1
            <A 12 MATERIAL_KIND >
            <A 16 MATERIAL_ID >
            <A 2 SLOTNO >
          >
        >
      >
    >
  >
>

<S6F101 P S6F101VariableDataNameList
  <L 2 L1
    <A 28 MODULEID >
    <U2 1 RPTUNIT >
  >
>

<S6F102 S S6F102VariableDataReply
  <L 3 L1
    <A 28 MODULEID >
    <U2 1 RPTUNIT >
    <L n VDATACNT
      <A 40 DATA_ITEM >
    >
  >
>

<S8F1 P S8F1MultiUseDataSet
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 2 L3
        <A 10 DATA_TYPE >
        <L 3 L4
          <A 40 ITEM_NAME >
          <A 100 ITEM_VALUE >
          <A 20 REFERENCE >
        >
      >
    >
  >
>

<S8F3 P S8F3
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 2 L3
        <A 10 DATA_TYPE >
        <L n L4
          <A 40 ITEM_NAME >
        >
      >
    >
  >
>

<S8F2 S S8F2
  <L 2 L1
    <U1 1 ACK >
    <L n L2
      <L 3 L3
        <A 10 DATA_TYPE >
        <L 3 L4
          <A 40 ITEM_NAME >
          <A 100 ITEM_VALUE >
          <A 20 REFERENCE >
        >
        <U1 1 EAC >
      >
    >
  >
>

<S8F4 S S8F4
  <L 2 L1
    <U1 1 ACK >
    <L n L2
      <L 2 L3
        <A 10 DATA_TYPE >
        <L 3 L4
          <A 40 ITEM_NAME >
          <A 100 ITEM_VALUE >
          <A 20 REFERENCE >
        >
      >
    >
  >
>

<S16F1 P S16F1
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F2 S S16F2
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 ACKC16 >
    <L n L2
      <L 4 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 16 SET_TIME >
        <L n L4
          <L 3 L5
            <A 28 P_MODULEID >
            <A 2 P_ORDER >
            <U1 1 P_STATUS >
          >
        >
      >
    >
  >
>

<S16F3 P S16F3
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 3 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <L n L4
          <L 2 L5
            <A 28 P_MODULEID >
            <A 2 P_ORDER >
          >
        >
      >
    >
  >
>

<S16F4 S S16F4
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 ACKC16 >
    <L n L2
      <L 3 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <L n L4
          <L 2 L5
            <A 28 P_MODULEID >
            <A 2 P_ORDER >
          >
        >
      >
    >
  >
>

<S16F5 P S16F5
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F6 S S16F6
  <U1 1 ACK16 >
>

<S16F11 P S16F11
  <L 2 L1
    <A 28 MODULEID >
    <L 4 L2
      <A 16 H_GLASSID >
      <A 16 JOBID >
      <A 16 SET_TIME >
      <L 1 L3
        <L 3 L4
          <A 28 P_MODULEID >
          <A 2 P_ORDER >
          <U1 1 P_STATUS >
        >
      >
    >
  >
>

<S16F12 S S16F12
  <U1 1 ACKC16 >
>

<S16F15 P S16F15
  <L 4 L1
    <A 28 MODULEID >
    <U1 1 MODE >
    <U1 1 BYWHO >
    <L n L2
      <L 4 L2_1
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 16 SET_TIME >
        <L n L3
          <L 3 L4
            <A 28 P_MODULEID >
            <A 2 P_ORDER >
            <U1 1 P_STATUS >
          >
        >
      >
    >
  >
>

<S16F16 S S16F16
  <U1 1 ACKC16 >
>

<S16F101 P S16F101
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F102 S S16F102
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 TCACK >
    <L n L2
      <L 6 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <A 16 SET_TIME >
        <U1 1 APC_STATE >
        <L n L4
          <L 2 L5
            <A 40 P_PARM_NAME >
            <A 40 P_PARM_VALUE >
          >
        >
      >
    >
  >
>

<S16F103 P S16F103
  <L 2 L1
    <A 28 MODULEID >
    <L n GLSCNT
      <L 4 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <L n PARAMCNT
          <L 2 L5
            <A 40 P_PARM_NAME >
            <A 40 P_PARM_VALUE >
          >
        >
      >
    >
  >
>

<S16F104 S S16F104
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 TCACK >
    <L n GLSCNT
      <L 5 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <A 16 SET_TIME >
        <L n PARAMCNT
          <L 2 L5
            <L 2 L6
              <A 40 P_PARM_NAME >
              <U1 1 PACK1 >
            >
            <L 2 L7
              <A 40 P_PARM_VALUE >
              <U1 1 PACK2 >
            >
          >
        >
      >
    >
  >
>

<S16F105 P S16F105
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F106 S S16F106
  <U1 1 TCACK >
>

<S16F111 P S16F111
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 5 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <A 16 SET_TIME >
        <L n L4
          <L 2 L5
            <A 40 P_PARM_NAME >
            <A 40 P_PARM_VALUE >
          >
        >
      >
    >
  >
>

<S16F112 S S16F112
  <U1 1 TCACK >
>

<S16F113 P S16F113
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 5 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <A 16 SET_TIME >
        <L n L4
          <L 2 L5
            <A 40 P_PARM_NAME >
            <A 40 P_PARM_VALUE >
          >
        >
      >
    >
  >
>

<S16F114 S S16F114
  <U1 1 TCACK >
>

<S16F115 P S16F115
  <L 4 L1
    <A 28 MODULEID >
    <U1 1 MODE >
    <U1 1 BYWHO >
    <L n L2
      <L 5 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RECIPE >
        <A 16 SET_TIME >
        <L n L4
          <L 2 L5
            <A 40 P_PARM_NAME >
            <A 40 P_PARM_VALUE >
          >
        >
      >
    >
  >
>

<S16F116 S S16F116
  <U1 1 TCACK >
>

<S16F121 P S16F121
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F122 S S16F122
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 ACKC16 >
    <L n L2
      <L 5 L4
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
        <A 16 SET_TIME >
        <U1 1 RPC_STATE >
      >
    >
  >
>

<S16F123 P S16F123
  <L 2 L1
    <A 28 MODULEID >
    <L n GLSCNT
      <L 3 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
      >
    >
  >
>

<S16F124 S S16F124
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 ACKC16 >
    <L n GLSCNT
      <L 4 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
        <A 16 SET_TIME >
      >
    >
  >
>

<S16F125 P S16F125
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <A 16 H_GLASSID >
    >
  >
>

<S16F126 S S16F126
  <U1 1 ACK16 >
>

<S16F131 P S16F131
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 4 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
        <A 16 SET_TIME >
      >
    >
  >
>

<S16F132 S S16F132
  <U1 1 ACKC16 >
>

<S16F133 P S16F133
  <L 2 L1
    <A 28 MODULEID >
    <L n L2
      <L 4 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
        <A 16 SET_TIME >
      >
    >
  >
>

<S16F134 S S16F134
  <U1 1 ACKC16 >
>

<S16F135 P S16F135
  <L 3 L1
    <A 28 MODULEID >
    <U1 1 MODE >
    <L n L2
      <L 5 L3
        <A 16 H_GLASSID >
        <A 16 JOBID >
        <A 20 RPC_PPID >
        <A 16 SET_TIME >
        <U1 1 BYWHO >
      >
    >
  >
>

<S16F136 S S16F136
  <U1 1 ACKC16 >
>

