# File name: save.plt - saves a Gnuplot plot as a PostScript file
# to save the current plot as a postscript file issue the commands:
#  gnuplot>   load 'saveplot'
#  gnuplot>   !mv my-plot.ps another-file.ps
set size 1.0, 0.6
#set terminal postscript portrait enhanced mono dashed lw 1 "Bitstream Vera Sans" 14
set terminal postscript eps portrait enhanced colour
set output "my-plot.ps"
replot
set terminal x11
set size 1,1
