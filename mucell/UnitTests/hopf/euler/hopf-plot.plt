set xlabel 'Time'
set ylabel 'Amount'
set title 'Hopf'
set grid
plot 'output_X.dat' using 1:2 title 'X' with lines, \
        'output_Z.dat' using 1:2 title 'Z' with lines, \
        'output_Y.dat' using 1:2 title 'Y' with lines
load 'save.plt'
pause 100
