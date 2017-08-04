from IpcPythonCS.RPC.RPCWrapper import RPCWrapper
from IpcPythonCS.Communication.ICommunicator import ICommunicator
from Module.OpenWeather import *

class PyRPCMethods(RPCWrapper):
    _communicator = ICommunicator

    def __init__(self, communicator):
        self._communicator = communicator
        return

    def Addition(self, a, b):
        return a + b

    def Subtraction(self, a, b):
        return a - b

    def GetWeatherByLocation(self, locName):
        return getWeatherByLocation(locName)