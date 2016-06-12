$(function() {


	vm.init();
});



var vm = {};

vm.init = function()
{
	bindClickEvents();
	bindDataTable();
};

var bindClickEvents = function()
{
	//open the lateral panel
	$('.cd-btn').on('click', function(event){
		event.preventDefault();
		$('.modal').addClass('is-open');
		$('body').addClass('modal-open');
	});
	//close the lateral panel
	$('.modal').on('click', function(event){
		if( $(event.target).is('.modal') || $(event.target).is('.modal-close') ) { 
			
			$('.modal').addClass('animate-out');
			$('.modal').removeClass('is-open');
			
			$('body').removeClass('modal-open');
			event.preventDefault();
		}
	});

};

var bindDataTable = function()
{
	// kickstart jQuery DataTable
    $('#example').DataTable({
    	"ajax": '/Home/SubmittedTasks',
    	"order": [[ 4, "desc" ]],
    	"columnDefs": [{
		    "defaultContent": "-",
		    "targets": "_all"
		  },
		  {
		  	"targets": [0],
		  	"data": "Id"
		  },
		  {
		  	"targets": [1],
		  	"data": "Name"
		  },
		  {
		  	"targets": [2],
		  	"data": "Type"
		  },
		  {
		  	"targets": [3],
		  	"data": "Status"
		  },
		  {
		  	"targets": [4],
		  	"data": "CreatedDate"
		  },
		  {
		  	"targets": [5],
		  	"data": "CreatedBy"
		  },
		  {
		  	"targets": [6],
		  	"data": "FileURL"
		  }]
    });
};




function singleFileSelected(evt) {
    //var selectedFile = evt.target.files can use this  or select input file element 
    //and access it's files object
    var selectedFile = ($("#UploadedFile"))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var FileSize = 0;
        var imageType = /image.*/;
        if (selectedFile.size > 1048576) {
            FileSize = Math.round(selectedFile.size * 100 / 1048576) / 100 + " MB";
        }
        else if (selectedFile.size > 1024) {
            FileSize = Math.round(selectedFile.size * 100 / 1024) / 100 + " KB";
        }
        else {
            FileSize = selectedFile.size + " Bytes";
        }


		// here we will add the code of thumbnail preview of upload images
		//  if (selectedFile.type.match(imageType)) {
		//     var reader = new FileReader();
		//     reader.onload = function (e) {

		//         $("#Imagecontainer").empty();
		//         var dataURL = reader.result;
		//         var img = new Image()
		//         img.src = dataURL;
		//         img.className = "thumb";
		//         $("#Imagecontainer").append(img);
		//     };
		//     reader.readAsDataURL(selectedFile);
		// }
       
        $("#FileName").text("Name : " + selectedFile.name);
        $("#FileType").text("type : " + selectedFile.type);
        $("#FileSize").text("Size : " + FileSize);
    }
}

function UploadFile() {
    //we can create form by passing the form to Constructor of formData object
    //or creating it manually using append function 
    //but please note file name should be same like the action Parameter
    //var dataString = new FormData();
    //dataString.append("UploadedFile", selectedFile);

    var form = $('#FormUpload')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: '/Home/SubmitTask',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                //myXhr.upload.onprogress = progressHandlingFunction
                myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events
        success: function(data){ console.log("success"); console.log(data); },
        error: function(data){ console.log("error"); console.log(data); },
        complete: function(data){ console.log("complete"); console.log(data); },
        // Form data
        data: dataString,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });
}

function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        var percentComplete = Math.round(e.loaded * 100 / e.total);
        $("#FileProgress").css("width", 
        percentComplete + '%').attr('aria-valuenow', percentComplete);
        $('#FileProgress span').text(percentComplete + "%");
    }
    else {
        $('#FileProgress span').text('unable to compute');
    }
}