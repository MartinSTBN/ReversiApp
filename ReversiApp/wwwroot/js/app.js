"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var apiUrl = 'url/super/duper/game';
console.log("TEST");

var Game = function (url) {
  var configMap = {
    apiUrl: url
  };
  var stateMap = {
    gameState: 0
  };

  var privateInit = function privateInit(id) {
    console.log(configMap.apiUrl);
    this.Reversi.init();
    this.Reversi.placeFiche(id); // this.Data.init();
    // this.Model.init();
    // callbackFunction();
    //window.setInterval(_getCurrentGameState, 2000);
  };

  var placeFiche = function placeFiche(id) {
    this.Reversi.placeFiche(id);
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    Game.Model.getGameState().then(function (array) {
      stateMap.gameState = array[0].data; //console.log('Current gamestate = ' + array[0].data);
    });
  };

  return {
    init: privateInit,
    currentGameState: _getCurrentGameState,
    placeFiche: placeFiche
  };
}(apiUrl);

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "show",
    value: function show(message, type) {
      var x = document.getElementById(this._elementId);
      var id = "#" + this._elementId;

      if (x.style.display === "none") {
        x.style.display = "block";
      }

      if (type == "success") {
        $(id).removeClass('alert-danger');
        $(id).addClass('alert-success');
      }

      if (type == "danger") {
        $(id).removeClass('alert-success');
        $(id).addClass('alert-danger');
      } //$(id).text(this.history()); 


      var param = {
        message: message,
        type: type
      };
      this.log(param);
    }
  }, {
    key: "hide",
    value: function hide() {
      $("." + this._elementId).fadeOut();
    }
  }, {
    key: "log",
    value: function log(message) {
      var existing = localStorage.getItem('feedback_widget');

      if (existing != null) {
        var values = existing.split(',');

        if (values.length < 10) {
          existing = existing ? JSON.parse(existing) : {};
          existing[message.type] = message.message;
          localStorage.setItem('feedback_widget', JSON.stringify(existing));
        } else {
          console.log('feedback_widget storage limit reached');
        }
      } else {
        existing = existing ? JSON.parse(existing) : {};
        existing[message.type] = message.message;
        localStorage.setItem('feedback_widget', JSON.stringify(existing));
      }
    }
  }, {
    key: "removeLog",
    value: function removeLog(key) {
      localStorage.removeItem(key);
    }
  }, {
    key: "history",
    value: function history() {
      var string = "";
      var existing = JSON.parse(localStorage.getItem('feedback_widget'));

      for (var key in existing) {
        string = string + "<type |" + key + "|>  -  <" + existing[key] + ">\n\n";
      }

      console.log(string);
      return string;
    }
  }, {
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId;
    }
  }]);

  return FeedbackWidget;
}();

Game.Data = function (url) {
  var configMap = {
    url: "http://api.openweathermap.org/data/2.5/weather?q=zwolle&apikey=2f86f292a0a338f6e118a047474c372e",
    mock: [{
      url: "api/Spel/Beurt",
      data: 0
    }]
  };
  var stateMap = {
    environment: 'production'
  };

  var privateInit = function privateInit(environment) {
    console.log('Data initiated.');
    console.log(environment);

    if (environment == 'production') {
      stateMap.environment = environment;
      return $.get(url).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else if (environment == 'development') {
      return getMockData(url);
    } else {
      throw new Error('Environment moet de waarde "production" of "development" hebben.');
    }
  };

  var get = function get(url) {
    if (stateMap.environment == 'development') {
      return getMockData(url);
    } else if (stateMap.environment == 'production') {
      return $.get(url).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    }
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  return {
    init: privateInit,
    get: get
  };
}(apiUrl);

Game.Model = function (url) {
  var configMap = {
    apiUrl: url
  };

  var privateInit = function privateInit() {
    console.log('Model initiated.');
  };

  var getWeather = function getWeather(key) {
    return Game.Data.get(key).then(function (data) {
      if (!data.main.temp) {
        console.log(data);
      } else {
        throw new Error('Geen temperatuur ontvangen.');
      }
    })["catch"](function (e) {
      console.log("Error opgevangen, error bericht: ".concat(e.message));
    });
  };

  var _getGameState = function _getGameState() {
    var url = "api/Spel/Beurt/<token>"; //aanvraag via Game.Data

    var state = Game.Data.get(url); //controle of ontvangen data valide is 

    if (state.data <= 0 || state.data > 2) {
      throw new Error('Waarde ' + state.data + ' valt buiten de geldige waarde!');
    } else {
      return state;
    }
  };

  return {
    init: privateInit,
    getWeather: getWeather,
    getGameState: _getGameState
  };
}(apiUrl);

Game.Reversi = function (url) {
  var configMap = {
    apiUrl: url
  };

  var privateInit = function privateInit() {
    console.log('Reversi initiated.');
  };

  var placeFiche = function placeFiche(kleur) {
    $.ajax({
      url: '/Game/Reversi',
      type: 'POST',
      dataType: 'json',
      data: {
        kleur: kleur
      },
      success: function success(data) {
        console.log(data);
      }
    });
  };

  return {
    init: privateInit,
    placeFiche: placeFiche
  };
}(apiUrl);