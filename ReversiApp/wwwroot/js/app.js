"use strict";function _classCallCheck(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}function _defineProperties(e,t){for(var n=0;n<t.length;n++){var a=t[n];a.enumerable=a.enumerable||!1,a.configurable=!0,"value"in a&&(a.writable=!0),Object.defineProperty(e,a.key,a)}}function _createClass(e,t,n){return t&&_defineProperties(e.prototype,t),n&&_defineProperties(e,n),e}var string=window.location.pathname,split=string.split("/"),id=split[3],apiUrl=window.location.origin+"/api/spel/beurt/"+id;console.log(apiUrl);var Game=function(){var e={apiUrl:apiUrl},t={gameState:0};return{init:function(e){this.Reversi.init(),this.Data.init(e),this.Model.init()},currentGameState:function(){Game.Model.getGameState(e.apiUrl).then(function(e){t.gameState=e,console.log(t.gameState)})},placeFiche:function(e,t){this.Reversi.placeFiche(e,t)},setTitle:function(e){this.Reversi.setTitle(e)},getTemplate:function(e){this.Template.getTemplate(e)},parseTemplate:function(e,t){this.Template.parseTemplate(e,t)}}}(),FeedbackWidget=function(){function t(e){_classCallCheck(this,t),this._elementId=e}return _createClass(t,[{key:"show",value:function(e,t){var n=document.getElementById(this._elementId),a="#"+this._elementId;"none"===n.style.display&&(n.style.display="block"),"success"==t&&($(a).removeClass("alert-danger"),$(a).addClass("alert-success")),"danger"==t&&($(a).removeClass("alert-success"),$(a).addClass("alert-danger"));var i={message:e,type:t};this.log(i)}},{key:"hide",value:function(){$("."+this._elementId).fadeOut()}},{key:"log",value:function(e){var t=localStorage.getItem("feedback_widget");null!=t?t.split(",").length<10?((t=t?JSON.parse(t):{})[e.type]=e.message,localStorage.setItem("feedback_widget",JSON.stringify(t))):console.log("feedback_widget storage limit reached"):((t=t?JSON.parse(t):{})[e.type]=e.message,localStorage.setItem("feedback_widget",JSON.stringify(t)))}},{key:"removeLog",value:function(e){localStorage.removeItem(e)}},{key:"history",value:function(){var e="",t=JSON.parse(localStorage.getItem("feedback_widget"));for(var n in t)e=e+"<type |"+n+"|>  -  <"+t[n]+">\n\n";return console.log(e),e}},{key:"elementId",get:function(){return this._elementId}}]),t}();Game.Data=function(){var e={url:apiUrl,mock:[{url:apiUrl,data:0}]},t={environment:"development"},n=function(){var n=e.mock;return new Promise(function(e,t){e(n)})};return{init:function(e){t.environment=e},get:function(e){if("production"==t.environment)return $.get(e).then(function(e){return e}).catch(function(e){console.log(e.message)});if("development"==t.environment)return n(e);throw new Error('Environment moet de waarde "production" of "development" hebben.')}}}(),Game.Model={init:function(){},getWeather:function(e){return Game.Data.get(e).then(function(e){if(e.main.temp)throw new Error("Geen temperatuur ontvangen.");console.log(e)}).catch(function(e){console.log("Error opgevangen, error bericht: ".concat(e.message))})},getGameState:function(e){var t=Game.Data.get(e);if(t.data<=0||2<t.data)throw new Error("Waarde "+t.data+" valt buiten de geldige waarde!");return t}},Game.Reversi={init:function(){},placeFiche:function(e,t){var n=document.createElement("div");1==t&&(n.className="fiche-wit",n.id=e),2==t&&(n.className="fiche-zwart",n.id=e),document.getElementById(e).appendChild(n)},setTitle:function(e){document.getElementById("AanDeBeurt").textContent="Player "+e+" is aan de beurt"}},Game.Template={getTemplate:function(e){var t=Handlebars.compile(e);return console.log(t),t("test"),t},parseTemplate:function(e,t){return t}};