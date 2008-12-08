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

def f(X, Y, Z):
		A = 1.0
		void = 0.0

		#R1 = k_1*X*A | X + A -> 2X
		R1 = k_1*X*A

		#R2 = k_2*X*Y | X + Y -> A + Y
		R2 = k_2*X*Y

		#R3 = k_3*X | X -> Z
		R3 = k_3*X

		#R4 = k_4*Z | Z -> Y
		R4 = k_4*Z

		#R5 = k_5*Y | Y -> void
		R5 = k_5*Y	
		
		delta_X = R1 - R2 - R3
		delta_Y = R4 - R5
		delta_Z = R3 - R4
		
#		R1v.append(R1)
#		R2v.append(R2)
#		R3v.append(R3)
#		R4v.append(R4)
#		R5v.append(R5)
		
		return (delta_X, delta_Y, delta_Z)

# Parameters
k_1 = 3.2
k_2 = 1.0
k_3 = 1.0
k_4 = 1.0
k_5 = 1.0
T = 18

# Initial conditions
X = [2.5]
A = 1.0
Y = [2.5]
Z = [2.5]
void = 0

h = 0.01001

for t in xrange(0, int(T/h)):
		
		(k1_dx, k1_dy, k1_dz) = f(X[t], Y[t], Z[t])
		(k2_dx, k2_dy, k2_dz) = f(X[t]+k1_dx*(h/2), Y[t]+k1_dy*(h/2), Z[t]+k1_dz*(h/2))
		(k3_dx, k3_dy, k3_dz) = f(X[t]+k2_dx*(h/2), Y[t]+k2_dy*(h/2), Z[t]+k2_dz*(h/2))
		(k4_dx, k4_dy, k4_dz) = f(X[t]+h*k3_dx, Y[t]+h*k3_dy, Z[t]+h*k3_dz)

		slope_dx = (h/6)*(k1_dx+2*k2_dx+2*k3_dx+k4_dx)
		slope_dy = (h/6)*(k1_dy+2*k2_dy+2*k3_dy+k4_dy)
		slope_dz = (h/6)*(k1_dz+2*k2_dz+2*k3_dz+k4_dz)

		X.append(X[t] + slope_dx)
		Y.append(Y[t] + slope_dy)
		Z.append(Z[t] + slope_dz)

toFile(X, "output_X.dat")
toFile(Y, "output_Y.dat")
toFile(Z, "output_Z.dat")
#toFile(R1v, "output_R1.dat")
#toFile(R2v, "output_R2.dat")
#toFile(R3v, "output_R3.dat")
#toFile(R4v, "output_R4.dat")
#toFile(R5v, "output_R5.dat")

os.system("gnuplot hopf-plot.plt")



