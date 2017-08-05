from Module.OpenWeather import *

class PyRPCMethods():
    def __init__(self):
        return

    def Addition(self, a, b):
        return a + b

    def Subtraction(self, a, b):
        return a - b

    def GetWeatherByLocation(self, locName):
        return getWeatherByLocation(locName)