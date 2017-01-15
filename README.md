# Hpgl2Spgl
Convert HPGL to SPGL as used by the Sweet-P SP-100/Heathkit IR-5208 plotter.

The Sweet-P SP-100 and Heathkit IR-5208 plotters are the same, but the Heathkit version was actually offered as a kit for people to assemble themselves. The SP-100 uses its own command language similar HPGL. This converter is for taking standard HPGL as produced by programs like Designspark PCB or AutoCAD and converting it to this different language. Note that one of the main missing features of the SPGL command set is any way to draw arcs and circles. Converting an arc/circle command to a sequence of line commands is to be added later.

SPGL commands are as follows, based on the Heathkit manual (most aren't going to be very useful):
MA x,y - move pen to absolute coordinate x,y (pen must be up)
DA x,y - move pen to absolute coordinate x,y (pen must be down)
MR x,y - move pen to relative coordinate x,y (pen must be up)
DR x,y - move pen to relative coordinate x,y (pen must be down)
LN x1,y1,x2,y2 - draw a line from point to point (unknown if pen must be down or not)
PT x,y - put a dot at x,y
PU - pen up, no coordinates
PD - pen down, no coordinates
RE - reset plotter (go to origin, reset everything to defaults)
HO - go to absolute coordinate 0,0
PL - set page length (including 1/2" margins)
VS - set velocity (see chart) - similar to HPGL, but speeds may not have the same meaning
TX - draw text at the current coordinate
TD - set text delimiter character
CS - set current text size in plotter units (0.004") - characters are 22x20 plotter units
MK - write a character centered at the current coordinate
RO - rotate text - only supports 0, 90, 180, and 270
AX - draw X axis with tick marks
AY - draw Y axis with tick marks

These commands are passed through as-is:
-IN
-PU with no coordinate
-PD with no coordinate
-VS

These commands are converted:
-PUxxx,xxx; becomes PU; Mx xxx,xxx; where Mx is either MA or MR, depending on if in absolute or relative mode
-PDxxx,xxx; becomes PD; Dx xxx,xxx; where Dx is either DA or DR, depending on if in absolute or relative mode
-PA with no coordinate changes to absolute mode
-PAxxx,xxx; becomes xA xxx,xxx; where xA is either MA or DA, depending on if the pen is up or down
-PR with no coordinate changes to relative mode
-PRxxx,xxx; becomes xR xxx,xxx; where xR is either MR or DR, depending on if the pen is up or down

These commands will be silently discarded:
-everything not in the two lists above

These commands are to be added at a later date:
-AA
-AR
-CI

VS velocity chart (from Heathkit manual):
0  - 1.40ips
1  - 1.55ips
2  - 1.70ips
3  - 1.89ips
4  - 2.13ips
5  - 2.35ips
6  - 2.62ips
7  - 2.92ips
8  - ??? (not documented)
9  - 3.65ips
10 - 4.13ips
11 - 4.61ips
12 - 5.06ips
13 - 5.46ips
14 - 5.72ips
15 - 6.00ips
