import os, os.path, re, sys, pprint

startDir =  os.getcwd()
directories = [startDir]

stats = []

def countLines(filename):
    lines = file(filename).readlines()
    numLines = len(lines)
    commentCount = 0
    blankCount = 0

    thisFile = {}
    thisFile["name"] = os.path.split(filename)[1]
    thisFile["lines"] = numLines

    for line in lines:
        if re.match("^(\s)*$", line):
            blankCount += 1
        elif re.match("^(\s)*((//)|(/\*))", line):
            commentCount += 1

    thisFile["blank"] = blankCount
    thisFile["comments"] = commentCount
    
    stats.append(thisFile)    

    return    


while len(directories)>0:
    directory = directories.pop()
    for name in os.listdir(directory):
        fullpath = os.path.join(directory,name)
        if os.path.isfile(fullpath):
            if fullpath.endswith(".cs"):
                countLines(fullpath)
        elif os.path.isdir(fullpath):
            directories.append(fullpath)

saveout = sys.stdout
fsock = open('metrics.log','w')
sys.stdout = fsock

blanks = 0
comments = 0
nlines = 0


print "Lines\tBlanks\tComments\tFilename"
print "-----\t------\t--------\t--------"
for stat in stats:
    print stat["lines"],
    print "\t",
    print stat["blank"],
    print "\t",
    print stat["comments"],
    print "\t   ",
    print stat["name"]
    blanks+=stat["blank"]
    comments+=stat["comments"]
    nlines+=stat["lines"]

print "%s \t %s \t %s \n"%(nlines, blanks, comments)

sys.stdout = saveout
