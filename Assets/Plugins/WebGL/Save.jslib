mergeInto(LibraryManager.library, {
  DownloadFile: function(byteArray, byteLength, fileName) {
    var blob = new Blob([HEAPU8.subarray(byteArray, byteArray + byteLength)], { type: "image/png" });
    var url = URL.createObjectURL(blob);

    var a = document.createElement("a");
    a.style.display = 'none';
    a.href = url;
    a.download = Pointer_stringify(fileName);
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  },
  LoadJSONFile: function(callback) {
    var inputElement = document.createElement('input');
    inputElement.type = 'file';
    inputElement.accept = '.json';
    inputElement.onchange = function(event) {
      var file = event.target.files[0];
      if (file) {
        var reader = new FileReader();
        reader.onload = function(loadEvent) {
          var contents = loadEvent.target.result;

          var lengthBytes = lengthBytesUTF8(contents) + 1;
          var buffer = _malloc(lengthBytes);
          stringToUTF8(contents, buffer, lengthBytes);

          {{{ makeDynCall('vi', 'callback') }}}(buffer);

          _free(buffer);
        };
        reader.readAsText(file);
      }
    };
    inputElement.click();
  }
});
