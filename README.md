# DesktopRenamer    
<br />

Very simple windows console application which scans all files in the directory it is started from and moves them to a folder corresponding to their starting literal.
For example, if a file 'test.txt' is found, the program looks for a folder named 'T' and moves the file to that folder.
If the folder doesn't exist, it will be created.


<br />

```
DesktopRenamer.exe [.] [(--dry-run|-dr)]

Version 1.0
- without any arguments, this help is displayed.
- the dot is mandatory to prevent accidentally running of the program.
- '--dry-run' or '-d' just writes logs, but doesn't touch a file or folder (case-insensitive).
```
<br />

*sagerobert*
