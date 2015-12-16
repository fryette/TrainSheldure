starter.filter("timeFilter", function () {
  return function (timeString) {
    var time = new Date(timeString)
      , hs = time.getHours()
      , mn = time.getMinutes();
    return (hs < 10 ? '0' : '') + hs + ':'
         + (mn < 10 ? '0' : '') + mn;
  }
});