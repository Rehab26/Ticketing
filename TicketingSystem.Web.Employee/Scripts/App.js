  
//new audio context to help us record 
var recordButton = document.getElementById("recordButton");
var stopButton = document.getElementById("stopButton"); 
//add events to buttons 
recordButton.addEventListener("click", startRecording);
stopButton.addEventListener("click", stopRecording); 
 
 

var constraints = {
    audio: true,
    video: false
} 
recordButton.disabled = false;
stopButton.disabled = true;
  

 var mediaRecorder;
function startRecording() {
    stopButton.disabled = false;
    recordButton.disabled = true;
    console.log("recording Start");
   
    navigator.mediaDevices.getUserMedia(constraints)
        .then(stream => {
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start();

            const audioChunks = [];
            mediaRecorder.addEventListener("dataavailable", event => {
                // merge all the chunk in one file 
                audioChunks.push(event.data);
            });

            mediaRecorder.addEventListener("stop", () => {
                debugger;
                // convert all the chunk in one blob file 
                const audioBlob = new Blob(audioChunks);
                ShowAndSave(audioBlob);
            });


        });
} 

function stopRecording() {
    console.log("recording Stopped");
    stopButton.disabled = true;
    recordButton.disabled = false;
    mediaRecorder.stop();
}


function ShowAndSave(blob) {
      
    var url = URL.createObjectURL(blob);
    var au = document.createElement('audio');
    var li = document.createElement('li');
    var link = document.createElement('a'); 
    au.controls = true;
    au.src = url; 
    link.href = url;
    link.download = new Date().toISOString() + '.wav';
    link.innerHTML = link.download; 
    li.appendChild(au);
    li.appendChild(link); 
    recordingsList.appendChild(li); 
 
   var formData = new FormData();
  formData.append("fileType", blob.type);
  formData.append("data", blob);
 //send file to the c# method 
   $.ajax({
       type: "POST",
       url: "/Account/SaveAudio/",
       data: formData,
       contentType: false, 
       processData: false,
       success: function () {
           alert("OK");
       },
       error: function () {
           alert("Error");
       }
   });  
    



}

 