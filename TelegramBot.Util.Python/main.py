from flask import Flask
from flask import request
from Module.PyRPCMethods import PyRPCMethods
import json
import os

# change working directory to current path (pycharm issue)
os.chdir(os.path.dirname(os.path.realpath(__file__)))

app = Flask(__name__)

@app.route("/")
def main():
    return "Hello"

@app.route("/main.php", methods=["GET", "POST"])
def hello():
    rtnStr = "지원하지 않는 명령어 입니다. /? 또는 /help 를 입력하여 사용법을 확인하세요."

    try:
        result = json.loads(request.data.decode("utf-8"))

        if (result["command"] == "날씨"):
            method = PyRPCMethods()
            rtnStr = method.GetWeatherByLocation(result["arg"])

        print(result)
    except:
        rtnStr = "해당 지역의 날씨 정보를 받아올 수 없습니다."
        pass

    return rtnStr

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5050)