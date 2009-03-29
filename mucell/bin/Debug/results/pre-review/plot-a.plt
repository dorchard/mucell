set xlabel 'Concentration (moles)'
set ylabel 'Average length (seconds)'
set title 'Tumble and Run length on concentrations'
set grid
plot 'a-run.dat' using 1:2 title 'Run time' with lines, \
	'a-tw.dat' using 1:2 title 'Tumble time' with lines
