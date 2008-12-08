# At time step of 1, performs the hopf.xml reaction
# equations and generates a gnuplot graph
# --- D. Orchard
# Uses Euler method to approximate

import os

# saves to file
def toFile(xs, filename):
        f = open(filename, "w")
        out = ""
        for t in range(0, int(T/h)):
                out+="%f %.16f\n"%(t, xs[t])
        f.write(out)
        f.close()
        return True

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
		
		return (delta_X, delta_Y, delta_Z)

# Parameters
k_1 = 3.2
k_2 = 1.0
k_3 = 1.0
k_4 = 1.0
k_5 = 1.0
T = 180

# Initial conditions
X = [2.5]
A = 1.0
Y = [2.5]
Z = [2.5]
void = 0

h = 0.01001

for t in xrange(0, int(T/h)):

		(dx, dy, dz) = f(X[t], Y[t], Z[t])

		X.append(X[t] + h*dx)
		Y.append(Y[t] + h*dy)
		Z.append(Z[t] + h*dz)

toFile(X, "output_X.dat")
toFile(Y, "output_Y.dat")
toFile(Z, "output_Z.dat")

os.system("gnuplot hopf-plot.plt")



