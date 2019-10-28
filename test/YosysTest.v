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
  output [3:0] io_out, // @[:@6.4]
  input        io_enable // @[:@6.4]
);
  reg [3:0] reg$; // @[YosysTest.scala 9:20:@8.4]
  reg [31:0] _RAND_0;
  wire [4:0] _T_7; // @[YosysTest.scala 12:15:@10.6]
  wire [3:0] _T_8; // @[YosysTest.scala 12:15:@11.6]
  wire [3:0] _GEN_0; // @[YosysTest.scala 11:18:@9.4]
  assign _T_7 = reg$ + 4'h1; // @[YosysTest.scala 12:15:@10.6]
  assign _T_8 = _T_7[3:0]; // @[YosysTest.scala 12:15:@11.6]
  assign _GEN_0 = io_enable ? _T_8 : reg$; // @[YosysTest.scala 11:18:@9.4]
  assign io_out = reg$;
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
    if (reset) begin
      reg$ <= 4'h0;
    end else begin
      if (io_enable) begin
        reg$ <= _T_8;
      end
    end
  end
endmodule
