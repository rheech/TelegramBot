import pyowm
from geopy.geocoders import Nominatim

def getWeatherByLocation(locName):
    owm = pyowm.OWM('apikey')

    geocoder = Nominatim()
    locInfo = geocoder.geocode(locName)

    foundLocName = str.format("{0},{1}", locInfo.city, locInfo.country_long)
    foundLocName = foundLocName.replace('None', '')

    foundLocNameShort = str.format("{0},{1}", locInfo.city, locInfo.country)
    foundLocNameShort = foundLocNameShort.replace('None', '')

    realLocName = "";

    if (foundLocName == '' or foundLocName == ','):
        foundLocName = locInfo.address
        foundLocNameShort = locInfo.address

    try:
        #observation = owm.weather_at_place(foundLocName)
        observation = owm.weather_at_coords(locInfo.lat, locInfo.lng)
        realLocName = observation.get_location().get_name()
        w = observation.get_weather()
        currentTemp = w.get_temperature('celsius')

        ss = str.format('{0} ({1}) 날씨\r\n현재 온도: {2}\u00B0C\r\n최고 온도: {3}\u00B0C\r\n최저 온도: {4}\u00B0C\r\n현재 습도: {5}%\r\n현재 상태: {6}',
                        realLocName, foundLocNameShort, currentTemp["temp"], currentTemp["temp_max"], currentTemp["temp_min"], w.get_humidity(), w.get_detailed_status())
    except:
        ss = '해당 지역의 날씨 정보를 받아올 수 없습니다.'

    return ss