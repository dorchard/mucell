# File name: save.plt - saves a Gnuplot plot as a PostScript file
# to save the current plot as a postscript file issue the commands:
#  gnuplot>   load 'saveplot'
#  gnuplot>   !mv my-plot.ps another-file.ps
set size 1.0, 1.0
#set terminal postscript enhanced mono dashed lw 1 "Bitstream Vera Sans" 14
set terminal postscript eps enhanced colour
set output "my-plot.eps"
replot
set terminal x11
set size 1,1
