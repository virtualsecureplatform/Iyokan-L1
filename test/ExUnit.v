`ifdef RANDOMIZE_GARBAGE_ASSIGN
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_INVALID_ASSIGN
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_REG_INIT
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_MEM_INIT
`define RANDOMIZE
`endif

module ExUnit( // @[:@3.2]
  input         clock, // @[:@4.4]
  input         reset, // @[:@5.4]
  input  [15:0] io_in_inA, // @[:@6.4]
  input  [15:0] io_in_inB, // @[:@6.4]
  input  [3:0]  io_in_opcode, // @[:@6.4]
  input  [2:0]  io_in_pcOpcode, // @[:@6.4]
  input  [15:0] io_in_pc, // @[:@6.4]
  input  [15:0] io_in_pcImm, // @[:@6.4]
  input         io_in_pcAdd, // @[:@6.4]
  input  [15:0] io_memIn_in, // @[:@6.4]
  input  [15:0] io_memIn_address, // @[:@6.4]
  input         io_memIn_memRead, // @[:@6.4]
  input         io_memIn_memWrite, // @[:@6.4]
  input         io_memIn_byteEnable, // @[:@6.4]
  input         io_memIn_signExt, // @[:@6.4]
  input  [3:0]  io_wbIn_regWrite, // @[:@6.4]
  input  [15:0] io_wbIn_regWriteData, // @[:@6.4]
  input         io_wbIn_regWriteEnable, // @[:@6.4]
  input         io_enable, // @[:@6.4]
  input         io_flush, // @[:@6.4]
  output [15:0] io_out_res, // @[:@6.4]
  output [8:0]  io_out_jumpAddress, // @[:@6.4]
  output        io_out_jump, // @[:@6.4]
  output [15:0] io_memOut_in, // @[:@6.4]
  output [15:0] io_memOut_address, // @[:@6.4]
  output        io_memOut_memRead, // @[:@6.4]
  output        io_memOut_memWrite, // @[:@6.4]
  output        io_memOut_byteEnable, // @[:@6.4]
  output        io_memOut_signExt, // @[:@6.4]
  output [3:0]  io_wbOut_regWrite, // @[:@6.4]
  output [15:0] io_wbOut_regWriteData, // @[:@6.4]
  output        io_wbOut_regWriteEnable // @[:@6.4]
);
  reg [15:0] pExReg_inA; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_0;
  reg [15:0] pExReg_inB; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_1;
  reg [3:0] pExReg_opcode; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_2;
  reg [2:0] pExReg_pcOpcode; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_3;
  reg [15:0] pExReg_pc; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_4;
  reg [15:0] pExReg_pcImm; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_5;
  reg  pExReg_pcAdd; // @[ExUnit.scala 50:23:@25.4]
  reg [31:0] _RAND_6;
  reg [15:0] pMemReg_in; // @[ExUnit.scala 51:24:@41.4]
  reg [31:0] _RAND_7;
  reg  pMemReg_memRead; // @[ExUnit.scala 51:24:@41.4]
  reg [31:0] _RAND_8;
  reg  pMemReg_memWrite; // @[ExUnit.scala 51:24:@41.4]
  reg [31:0] _RAND_9;
  reg  pMemReg_byteEnable; // @[ExUnit.scala 51:24:@41.4]
  reg [31:0] _RAND_10;
  reg  pMemReg_signExt; // @[ExUnit.scala 51:24:@41.4]
  reg [31:0] _RAND_11;
  reg [3:0] pWbReg_regWrite; // @[ExUnit.scala 52:23:@51.4]
  reg [31:0] _RAND_12;
  reg  pWbReg_regWriteEnable; // @[ExUnit.scala 52:23:@51.4]
  reg [31:0] _RAND_13;
  wire [15:0] _T_166; // @[ExUnit.scala 76:15:@58.4]
  wire [16:0] _T_168; // @[ExUnit.scala 76:36:@59.4]
  wire [15:0] inB_sub; // @[ExUnit.scala 76:36:@60.4]
  wire  _GEN_0; // @[ExUnit.scala 88:19:@84.6]
  wire  _GEN_1; // @[ExUnit.scala 88:19:@84.6]
  wire [2:0] _GEN_2; // @[ExUnit.scala 88:19:@84.6]
  wire  _GEN_3; // @[ExUnit.scala 84:19:@67.4]
  wire [15:0] _GEN_4; // @[ExUnit.scala 84:19:@67.4]
  wire [15:0] _GEN_5; // @[ExUnit.scala 84:19:@67.4]
  wire [2:0] _GEN_6; // @[ExUnit.scala 84:19:@67.4]
  wire [3:0] _GEN_7; // @[ExUnit.scala 84:19:@67.4]
  wire [15:0] _GEN_8; // @[ExUnit.scala 84:19:@67.4]
  wire [15:0] _GEN_9; // @[ExUnit.scala 84:19:@67.4]
  wire  _GEN_10; // @[ExUnit.scala 84:19:@67.4]
  wire  _GEN_11; // @[ExUnit.scala 84:19:@67.4]
  wire  _GEN_12; // @[ExUnit.scala 84:19:@67.4]
  wire  _GEN_13; // @[ExUnit.scala 84:19:@67.4]
  wire [15:0] _GEN_15; // @[ExUnit.scala 84:19:@67.4]
  wire  _GEN_16; // @[ExUnit.scala 84:19:@67.4]
  wire [3:0] _GEN_18; // @[ExUnit.scala 84:19:@67.4]
  wire  _T_176; // @[ExUnit.scala 100:22:@102.4]
  wire [16:0] _T_177; // @[ExUnit.scala 101:30:@104.6]
  wire [15:0] _T_178; // @[ExUnit.scala 101:30:@105.6]
  wire  _T_182; // @[ExUnit.scala 102:28:@110.6]
  wire [16:0] resCarry; // @[ExUnit.scala 103:28:@112.8]
  wire [15:0] _T_184; // @[ExUnit.scala 104:27:@114.8]
  wire  _T_188; // @[ExUnit.scala 105:28:@119.8]
  wire [15:0] _T_189; // @[ExUnit.scala 106:30:@121.10]
  wire  _T_193; // @[ExUnit.scala 107:28:@126.10]
  wire [15:0] _T_194; // @[ExUnit.scala 108:30:@128.12]
  wire  _T_198; // @[ExUnit.scala 109:28:@133.12]
  wire [15:0] _T_199; // @[ExUnit.scala 110:30:@135.14]
  wire  _T_203; // @[ExUnit.scala 111:28:@140.14]
  wire [65550:0] _GEN_39; // @[ExUnit.scala 112:31:@142.16]
  wire [65550:0] _T_204; // @[ExUnit.scala 112:31:@142.16]
  wire  _T_208; // @[ExUnit.scala 113:28:@147.16]
  wire [15:0] _T_209; // @[ExUnit.scala 114:31:@149.18]
  wire  _T_213; // @[ExUnit.scala 115:28:@154.18]
  wire [15:0] _T_214; // @[ExUnit.scala 116:38:@156.20]
  wire [15:0] _T_215; // @[ExUnit.scala 116:42:@157.20]
  wire [15:0] _T_216; // @[ExUnit.scala 116:63:@158.20]
  wire [15:0] _GEN_20; // @[ExUnit.scala 115:47:@155.18]
  wire [15:0] _GEN_21; // @[ExUnit.scala 113:47:@148.16]
  wire [65550:0] _GEN_22; // @[ExUnit.scala 111:47:@141.14]
  wire [65550:0] _GEN_23; // @[ExUnit.scala 109:47:@134.12]
  wire [65550:0] _GEN_24; // @[ExUnit.scala 107:46:@127.10]
  wire [65550:0] _GEN_25; // @[ExUnit.scala 105:47:@120.8]
  wire [65550:0] _GEN_27; // @[ExUnit.scala 102:47:@111.6]
  wire [65550:0] _GEN_28; // @[ExUnit.scala 100:41:@103.4]
  wire [16:0] _T_221; // @[ExUnit.scala 124:37:@171.6]
  wire [15:0] _T_222; // @[ExUnit.scala 124:37:@172.6]
  wire [15:0] _GEN_30; // @[ExUnit.scala 123:22:@170.4]
  wire  _T_223; // @[ExUnit.scala 129:25:@178.4]
  wire  flagCarry; // @[ExUnit.scala 129:16:@179.4]
  wire  flagSign; // @[ExUnit.scala 130:25:@181.4]
  wire  flagZero; // @[ExUnit.scala 131:27:@183.4]
  wire  _T_236; // @[ExUnit.scala 59:18:@189.4]
  wire  _T_237; // @[ExUnit.scala 60:18:@191.4]
  wire  _T_239; // @[ExUnit.scala 62:20:@195.4]
  wire  _T_241; // @[ExUnit.scala 62:31:@196.4]
  wire  _T_242; // @[ExUnit.scala 62:53:@197.4]
  wire  flagOverflow; // @[ExUnit.scala 62:40:@199.4]
  wire  _T_250; // @[ExUnit.scala 134:24:@208.4]
  wire  _T_252; // @[ExUnit.scala 136:30:@213.6]
  wire  _T_254; // @[ExUnit.scala 138:30:@218.8]
  wire  _T_255; // @[ExUnit.scala 139:29:@220.10]
  wire  _T_257; // @[ExUnit.scala 140:30:@224.10]
  wire  _T_260; // @[ExUnit.scala 142:30:@229.12]
  wire  _T_262; // @[ExUnit.scala 143:20:@231.14]
  wire  _T_264; // @[ExUnit.scala 144:30:@235.14]
  wire  _T_265; // @[ExUnit.scala 145:30:@237.16]
  wire  _T_267; // @[ExUnit.scala 146:30:@241.16]
  wire  _T_269; // @[ExUnit.scala 147:46:@244.18]
  wire  _GEN_32; // @[ExUnit.scala 146:38:@242.16]
  wire  _GEN_33; // @[ExUnit.scala 144:38:@236.14]
  wire  _GEN_34; // @[ExUnit.scala 142:38:@230.12]
  wire  _GEN_35; // @[ExUnit.scala 140:38:@225.10]
  wire  _GEN_36; // @[ExUnit.scala 138:38:@219.8]
  wire  _GEN_37; // @[ExUnit.scala 136:38:@214.6]
  wire  _GEN_38; // @[ExUnit.scala 134:32:@209.4]
  wire  _T_273; // @[ExUnit.scala 152:11:@249.6]
  assign _T_166 = ~ pExReg_inB; // @[ExUnit.scala 76:15:@58.4]
  assign _T_168 = _T_166 + 16'h1; // @[ExUnit.scala 76:36:@59.4]
  assign inB_sub = _T_168[15:0]; // @[ExUnit.scala 76:36:@60.4]
  assign _GEN_0 = io_flush ? 1'h0 : io_memIn_memWrite; // @[ExUnit.scala 88:19:@84.6]
  assign _GEN_1 = io_flush ? 1'h0 : io_wbIn_regWriteEnable; // @[ExUnit.scala 88:19:@84.6]
  assign _GEN_2 = io_flush ? 3'h0 : io_in_pcOpcode; // @[ExUnit.scala 88:19:@84.6]
  assign _GEN_3 = io_enable ? io_in_pcAdd : pExReg_pcAdd; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_4 = io_enable ? io_in_pcImm : pExReg_pcImm; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_5 = io_enable ? io_in_pc : pExReg_pc; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_6 = io_enable ? _GEN_2 : pExReg_pcOpcode; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_7 = io_enable ? io_in_opcode : pExReg_opcode; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_8 = io_enable ? io_in_inB : pExReg_inB; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_9 = io_enable ? io_in_inA : pExReg_inA; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_10 = io_enable ? io_memIn_signExt : pMemReg_signExt; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_11 = io_enable ? io_memIn_byteEnable : pMemReg_byteEnable; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_12 = io_enable ? _GEN_0 : pMemReg_memWrite; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_13 = io_enable ? io_memIn_memRead : pMemReg_memRead; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_15 = io_enable ? io_memIn_in : pMemReg_in; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_16 = io_enable ? _GEN_1 : pWbReg_regWriteEnable; // @[ExUnit.scala 84:19:@67.4]
  assign _GEN_18 = io_enable ? io_wbIn_regWrite : pWbReg_regWrite; // @[ExUnit.scala 84:19:@67.4]
  assign _T_176 = 4'h0 == pExReg_opcode; // @[ExUnit.scala 100:22:@102.4]
  assign _T_177 = pExReg_inA + pExReg_inB; // @[ExUnit.scala 101:30:@104.6]
  assign _T_178 = _T_177[15:0]; // @[ExUnit.scala 101:30:@105.6]
  assign _T_182 = 4'h1 == pExReg_opcode; // @[ExUnit.scala 102:28:@110.6]
  assign resCarry = pExReg_inA + inB_sub; // @[ExUnit.scala 103:28:@112.8]
  assign _T_184 = resCarry[15:0]; // @[ExUnit.scala 104:27:@114.8]
  assign _T_188 = 4'h2 == pExReg_opcode; // @[ExUnit.scala 105:28:@119.8]
  assign _T_189 = pExReg_inA & pExReg_inB; // @[ExUnit.scala 106:30:@121.10]
  assign _T_193 = 4'h4 == pExReg_opcode; // @[ExUnit.scala 107:28:@126.10]
  assign _T_194 = pExReg_inA | pExReg_inB; // @[ExUnit.scala 108:30:@128.12]
  assign _T_198 = 4'h3 == pExReg_opcode; // @[ExUnit.scala 109:28:@133.12]
  assign _T_199 = pExReg_inA ^ pExReg_inB; // @[ExUnit.scala 110:30:@135.14]
  assign _T_203 = 4'h5 == pExReg_opcode; // @[ExUnit.scala 111:28:@140.14]
  assign _GEN_39 = {{65535'd0}, pExReg_inA}; // @[ExUnit.scala 112:31:@142.16]
  assign _T_204 = _GEN_39 << pExReg_inB; // @[ExUnit.scala 112:31:@142.16]
  assign _T_208 = 4'h6 == pExReg_opcode; // @[ExUnit.scala 113:28:@147.16]
  assign _T_209 = pExReg_inA >> pExReg_inB; // @[ExUnit.scala 114:31:@149.18]
  assign _T_213 = 4'h7 == pExReg_opcode; // @[ExUnit.scala 115:28:@154.18]
  assign _T_214 = $signed(pExReg_inA); // @[ExUnit.scala 116:38:@156.20]
  assign _T_215 = $signed(_T_214) >>> pExReg_inB; // @[ExUnit.scala 116:42:@157.20]
  assign _T_216 = $unsigned(_T_215); // @[ExUnit.scala 116:63:@158.20]
  assign _GEN_20 = _T_213 ? _T_216 : pExReg_inB; // @[ExUnit.scala 115:47:@155.18]
  assign _GEN_21 = _T_208 ? _T_209 : _GEN_20; // @[ExUnit.scala 113:47:@148.16]
  assign _GEN_22 = _T_203 ? _T_204 : {{65535'd0}, _GEN_21}; // @[ExUnit.scala 111:47:@141.14]
  assign _GEN_23 = _T_198 ? {{65535'd0}, _T_199} : _GEN_22; // @[ExUnit.scala 109:47:@134.12]
  assign _GEN_24 = _T_193 ? {{65535'd0}, _T_194} : _GEN_23; // @[ExUnit.scala 107:46:@127.10]
  assign _GEN_25 = _T_188 ? {{65535'd0}, _T_189} : _GEN_24; // @[ExUnit.scala 105:47:@120.8]
  assign _GEN_27 = _T_182 ? {{65535'd0}, _T_184} : _GEN_25; // @[ExUnit.scala 102:47:@111.6]
  assign _GEN_28 = _T_176 ? {{65535'd0}, _T_178} : _GEN_27; // @[ExUnit.scala 100:41:@103.4]
  assign _T_221 = pExReg_pc + pExReg_pcImm; // @[ExUnit.scala 124:37:@171.6]
  assign _T_222 = _T_221[15:0]; // @[ExUnit.scala 124:37:@172.6]
  assign _GEN_30 = pExReg_pcAdd ? _T_222 : pExReg_pcImm; // @[ExUnit.scala 123:22:@170.4]
  assign _T_223 = resCarry[16]; // @[ExUnit.scala 129:25:@178.4]
  assign flagCarry = ~ _T_223; // @[ExUnit.scala 129:16:@179.4]
  assign flagSign = io_out_res[15]; // @[ExUnit.scala 130:25:@181.4]
  assign flagZero = io_out_res == 16'h0; // @[ExUnit.scala 131:27:@183.4]
  assign _T_236 = pExReg_inA[15]; // @[ExUnit.scala 59:18:@189.4]
  assign _T_237 = inB_sub[15]; // @[ExUnit.scala 60:18:@191.4]
  assign _T_239 = _T_236 ^ _T_237; // @[ExUnit.scala 62:20:@195.4]
  assign _T_241 = _T_239 == 1'h0; // @[ExUnit.scala 62:31:@196.4]
  assign _T_242 = _T_237 ^ flagSign; // @[ExUnit.scala 62:53:@197.4]
  assign flagOverflow = _T_241 & _T_242; // @[ExUnit.scala 62:40:@199.4]
  assign _T_250 = pExReg_pcOpcode == 3'h1; // @[ExUnit.scala 134:24:@208.4]
  assign _T_252 = pExReg_pcOpcode == 3'h2; // @[ExUnit.scala 136:30:@213.6]
  assign _T_254 = pExReg_pcOpcode == 3'h3; // @[ExUnit.scala 138:30:@218.8]
  assign _T_255 = flagCarry | flagZero; // @[ExUnit.scala 139:29:@220.10]
  assign _T_257 = pExReg_pcOpcode == 3'h4; // @[ExUnit.scala 140:30:@224.10]
  assign _T_260 = pExReg_pcOpcode == 3'h5; // @[ExUnit.scala 142:30:@229.12]
  assign _T_262 = flagZero == 1'h0; // @[ExUnit.scala 143:20:@231.14]
  assign _T_264 = pExReg_pcOpcode == 3'h6; // @[ExUnit.scala 144:30:@235.14]
  assign _T_265 = flagSign != flagOverflow; // @[ExUnit.scala 145:30:@237.16]
  assign _T_267 = pExReg_pcOpcode == 3'h7; // @[ExUnit.scala 146:30:@241.16]
  assign _T_269 = _T_265 | flagZero; // @[ExUnit.scala 147:46:@244.18]
  assign _GEN_32 = _T_267 ? _T_269 : 1'h0; // @[ExUnit.scala 146:38:@242.16]
  assign _GEN_33 = _T_264 ? _T_265 : _GEN_32; // @[ExUnit.scala 144:38:@236.14]
  assign _GEN_34 = _T_260 ? _T_262 : _GEN_33; // @[ExUnit.scala 142:38:@230.12]
  assign _GEN_35 = _T_257 ? 1'h1 : _GEN_34; // @[ExUnit.scala 140:38:@225.10]
  assign _GEN_36 = _T_254 ? _T_255 : _GEN_35; // @[ExUnit.scala 138:38:@219.8]
  assign _GEN_37 = _T_252 ? flagCarry : _GEN_36; // @[ExUnit.scala 136:38:@214.6]
  assign _GEN_38 = _T_250 ? flagZero : _GEN_37; // @[ExUnit.scala 134:32:@209.4]
  assign _T_273 = reset == 1'h0; // @[ExUnit.scala 152:11:@249.6]
  assign io_out_res = _GEN_28[15:0];
  assign io_out_jumpAddress = _GEN_30[8:0];
  assign io_out_jump = _GEN_38;
  assign io_memOut_in = pMemReg_in;
  assign io_memOut_address = io_out_res;
  assign io_memOut_memRead = pMemReg_memRead;
  assign io_memOut_memWrite = pMemReg_memWrite;
  assign io_memOut_byteEnable = pMemReg_byteEnable;
  assign io_memOut_signExt = pMemReg_signExt;
  assign io_wbOut_regWrite = pWbReg_regWrite;
  assign io_wbOut_regWriteData = io_out_res;
  assign io_wbOut_regWriteEnable = pWbReg_regWriteEnable;
`ifdef RANDOMIZE
  integer initvar;
  initial begin
    `ifndef verilator
      #0.002 begin end
    `endif
  `ifdef RANDOMIZE_REG_INIT
  _RAND_0 = {1{$random}};
  pExReg_inA = _RAND_0[15:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_1 = {1{$random}};
  pExReg_inB = _RAND_1[15:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_2 = {1{$random}};
  pExReg_opcode = _RAND_2[3:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_3 = {1{$random}};
  pExReg_pcOpcode = _RAND_3[2:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_4 = {1{$random}};
  pExReg_pc = _RAND_4[15:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_5 = {1{$random}};
  pExReg_pcImm = _RAND_5[15:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_6 = {1{$random}};
  pExReg_pcAdd = _RAND_6[0:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_7 = {1{$random}};
  pMemReg_in = _RAND_7[15:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_8 = {1{$random}};
  pMemReg_memRead = _RAND_8[0:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_9 = {1{$random}};
  pMemReg_memWrite = _RAND_9[0:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_10 = {1{$random}};
  pMemReg_byteEnable = _RAND_10[0:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_11 = {1{$random}};
  pMemReg_signExt = _RAND_11[0:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_12 = {1{$random}};
  pWbReg_regWrite = _RAND_12[3:0];
  `endif // RANDOMIZE_REG_INIT
  `ifdef RANDOMIZE_REG_INIT
  _RAND_13 = {1{$random}};
  pWbReg_regWriteEnable = _RAND_13[0:0];
  `endif // RANDOMIZE_REG_INIT
  end
`endif // RANDOMIZE
  always @(posedge clock) begin
    if (reset) begin
      pExReg_inA <= 16'h0;
    end else begin
      if (io_enable) begin
        pExReg_inA <= io_in_inA;
      end
    end
    if (reset) begin
      pExReg_inB <= 16'h0;
    end else begin
      if (io_enable) begin
        pExReg_inB <= io_in_inB;
      end
    end
    if (reset) begin
      pExReg_opcode <= 4'h0;
    end else begin
      if (io_enable) begin
        pExReg_opcode <= io_in_opcode;
      end
    end
    if (reset) begin
      pExReg_pcOpcode <= 3'h0;
    end else begin
      if (io_enable) begin
        if (io_flush) begin
          pExReg_pcOpcode <= 3'h0;
        end else begin
          pExReg_pcOpcode <= io_in_pcOpcode;
        end
      end
    end
    if (reset) begin
      pExReg_pc <= 16'h0;
    end else begin
      if (io_enable) begin
        pExReg_pc <= io_in_pc;
      end
    end
    if (reset) begin
      pExReg_pcImm <= 16'h0;
    end else begin
      if (io_enable) begin
        pExReg_pcImm <= io_in_pcImm;
      end
    end
    if (reset) begin
      pExReg_pcAdd <= 1'h0;
    end else begin
      if (io_enable) begin
        pExReg_pcAdd <= io_in_pcAdd;
      end
    end
    if (reset) begin
      pMemReg_in <= 16'h0;
    end else begin
      if (io_enable) begin
        pMemReg_in <= io_memIn_in;
      end
    end
    if (reset) begin
      pMemReg_memRead <= 1'h0;
    end else begin
      if (io_enable) begin
        pMemReg_memRead <= io_memIn_memRead;
      end
    end
    if (reset) begin
      pMemReg_memWrite <= 1'h0;
    end else begin
      if (io_enable) begin
        if (io_flush) begin
          pMemReg_memWrite <= 1'h0;
        end else begin
          pMemReg_memWrite <= io_memIn_memWrite;
        end
      end
    end
    if (reset) begin
      pMemReg_byteEnable <= 1'h0;
    end else begin
      if (io_enable) begin
        pMemReg_byteEnable <= io_memIn_byteEnable;
      end
    end
    if (reset) begin
      pMemReg_signExt <= 1'h0;
    end else begin
      if (io_enable) begin
        pMemReg_signExt <= io_memIn_signExt;
      end
    end
    if (reset) begin
      pWbReg_regWrite <= 4'h0;
    end else begin
      if (io_enable) begin
        pWbReg_regWrite <= io_wbIn_regWrite;
      end
    end
    if (reset) begin
      pWbReg_regWriteEnable <= 1'h0;
    end else begin
      if (io_enable) begin
        if (io_flush) begin
          pWbReg_regWriteEnable <= 1'h0;
        end else begin
          pWbReg_regWriteEnable <= io_wbIn_regWriteEnable;
        end
      end
    end
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] opcode:0x%h\n",pExReg_opcode); // @[ExUnit.scala 152:11:@251.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] inA:0x%h\n",pExReg_inA); // @[ExUnit.scala 153:11:@256.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] inB:0x%h\n",pExReg_inB); // @[ExUnit.scala 154:11:@261.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] Res:0x%h\n",io_out_res); // @[ExUnit.scala 155:11:@266.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] PC Address:0x%h\n",pExReg_pc); // @[ExUnit.scala 156:11:@271.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] Jump:%d\n",io_out_jump); // @[ExUnit.scala 157:11:@276.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
    `ifndef SYNTHESIS
    `ifdef PRINTF_COND
      if (`PRINTF_COND) begin
    `endif
        if (_T_273) begin
          $fwrite(32'h80000002,"[EX] JumpAddress:0x%h\n",io_out_jumpAddress); // @[ExUnit.scala 158:11:@281.8]
        end
    `ifdef PRINTF_COND
      end
    `endif
    `endif // SYNTHESIS
  end
endmodule
