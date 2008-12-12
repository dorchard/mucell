# At time step of 1, performs the hopf.xml reaction
# equations and generates a gnuplot graph
# --- D. Orchard
# Runge-Kutta technique

import os

# saves to file
def toFile(xs, filename):
        f = open(filename, "w")
        out = ""
        for t in range(0, int(T/h)):
                out+="%f %.16f\n"%(t*h, xs[t])
        f.write(out)
        f.close()
        return True
        
# rates
R1v = [0]
R2v = [0]
R3v = [0]
R4v = [0]
R5v = [0]

def f(s1, s2):
		R1 = s1*2

		delta_s1 = -R1 + R1*0.75
		delta_s2 = R1*2
		
		return (delta_s1, delta_s2)

# Parameters
T = 18

# Initial conditions
s1 = [4.0]
s2 = [0.1]

h = 0.01001

for t in xrange(0, int(T/h)):
		
		(k1_dx, k1_dy) = f(s1[t], s2[t])
		(k2_dx, k2_dy) = f(s1[t]+k1_dx*(h/2), s2[t]+k1_dy*(h/2))
		(k3_dx, k3_dy) = f(s1[t]+k2_dx*(h/2), s2[t]+k2_dy*(h/2))
		(k4_dx, k4_dy) = f(s1[t]+h*k3_dx, s2[t]+h*k3_dy)

		slope_dx = (h/6)*(k1_dx+2*k2_dx+2*k3_dx+k4_dx)
		slope_dy = (h/6)*(k1_dy+2*k2_dy+2*k3_dy+k4_dy)

		s1.append(s1[t] + slope_dx)
		s2.append(s2[t] + slope_dy)

toFile(s1, "output_X.dat")
toFile(s2, "output_Y.dat")
toFile(s1, "output_Z.dat")
#toFile(R1v, "output_R1.dat")
#toFile(R2v, "output_R2.dat")
#toFile(R3v, "output_R3.dat")
#toFile(R4v, "output_R4.dat")
#toFile(R5v, "output_R5.dat")

os.system("gnuplot hopf-plot.plt")



