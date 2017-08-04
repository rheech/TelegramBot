from Module.PyRPCMethods import PyRPCMethods
from IpcPythonCS.Communication.Pipe.PipeServer import PipeServer

print("Waiting for clients.")
server = PipeServer()

try:
    # Pipe가 이미 열려 있을 경우 발생하는 그냥 application 종료.
    server.WaitForConnection("pyrpcmethods")
except:
    exit()

calc = PyRPCMethods(server)

#print("hello")

## One time execution ##
try:
    while(True):
        calc.ProcessFunctionCall()
except:
    print("Connection ended.")
    server.Close()

## Infinite execution ##
'''
while (True):
    try:
        print("Connected.")
        calc.ProcessFunctionCall()
    except:
        print("Closed. Waiting for upcoming clients.")
        server.Close()
        server.WaitForConnection("pyrpcmethods")
        calc = PyRPCMethods(server)
'''