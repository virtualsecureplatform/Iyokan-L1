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

module YosysTest( // @[:@3.2]
  input        clock, // @[:@4.4]
  input        reset, // @[:@5.4]
  input  [3:0] io_in, // @[:@6.4]
  output [3:0] io_out // @[:@6.4]
);
  reg [3:0] reg$; // @[YosysTest.scala 10:16:@8.4]
  reg [31:0] _RAND_0;
  wire [4:0] _T_6; // @[YosysTest.scala 12:17:@10.4]
  wire [3:0] _T_7; // @[YosysTest.scala 12:17:@11.4]
  assign _T_6 = reg$ + 4'h1; // @[YosysTest.scala 12:17:@10.4]
  assign _T_7 = _T_6[3:0]; // @[YosysTest.scala 12:17:@11.4]
  assign io_out = _T_7;
`ifdef RANDOMIZE
  integer initvar;
  initial begin
    `ifndef verilator
      #0.002 begin end
    `endif
  `ifdef RANDOMIZE_REG_INIT
  _RAND_0 = {1{$random}};
  reg$ = _RAND_0[3:0];
  `endif // RANDOMIZE_REG_INIT
  end
`endif // RANDOMIZE
  always @(posedge clock) begin
    reg$ <= io_in;
  end
endmodule
