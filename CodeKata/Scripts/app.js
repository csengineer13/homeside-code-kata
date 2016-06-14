$(function() {


	vm.init();
});



var vm = {};

vm.init = function()
{
	bindClickEvents();
	bindSelect2();
	bindDataTable();
};

vm.Notifications = {
	add: function(message, type, id){
		// info, success, warning, error
		$newNotification = '<div class="notification notification--' + type + '">' + message + '</div>';
		$(".notification-container").append($newNotification);

		setTimeout(function(){ vm.Notifications.removeOldest(); }, 3000);
	},
	removeOldest: function(){
		$(".notification-container").find('.notification:first').remove();
	},
	removeAll: function(){
		$(".notification-container").html("");
	}
};

vm.Form = {
	clear: function(){
		$("#TaskType").val("");
	    $("#SubmittedBy").select2('val', 'All');
		$("#TaskName").val("");
		$("#TaskDescription").val("");
	    $('#FileToUpload').val("");
	}
};

vm.setTaskType = function(taskType)
{
	$("#TaskType").val(taskType);

	switch(taskType) {
	    case "PayMIP":
	        $(".modal-header h3").text("Pay MIP");
	        break;
	    case "PostTransactions":
	        $(".modal-header h3").text("Post Transactions");
	        break;
	    case "CreateWarehouseLineFile":
	        $(".modal-header h3").text("Create Warehouse Line File");
	        break;
	    default:
	        $(".modal-header h3").text("No Task Set");
	}
};

var bindClickEvents = function()
{
	//open the lateral panel
	$('.cd-btn').on('click', function(event)
	{
		event.preventDefault();
		var $target = $(event.target);
  		vm.setTaskType($target.attr('target'));

		$('.modal').addClass('is-open');
		$('body').addClass('modal-open');
	});
	//close the lateral panel
	$('.modal').on('click', function(event){
		if( $(event.target).is('.modal') || $(event.target).is('.modal-close') || $(event.target).is("#Close_btn") ) { 
			
			$('.modal').addClass('animate-out');
			$('.modal').removeClass('is-open');
			
			$('body').removeClass('modal-open');
			vm.Form.clear();
			event.preventDefault();
		}
	});

	$("#Submit_btn").on('click', UploadFile);
};

var bindSelect2 = function()
{
	$("#SubmittedBy").select2(
		{
			ajax: {
				url: "/Home/GetUsers",
				dataType: 'json',
				delay: 250, // debounce term search
				data: function(params){
					return {
						searchTerm: params.term, // search term
						page: params.page
					};
				},
				processResults: function(data, params){
					// You can parse results into your expected format here
				
					params.page = params.page || 1;

					return {
						results: data.items,
						pagination: {
							more: (params.page * 30) < data.total_count
						}
					};
				},
				cache: true
			},
			placeholder: "User Submitting Task",
			allowClear: true,
			templateResult: function(userDto){
				if(!userDto.Name) { return userDto.text; }
				var $userDto = $(
					'<div><b>' + userDto.Name + '</b></div><div>' + userDto.EmployeeId + '</div>'
					);
				return $userDto;
			},
			templateSelection: function(userDto){
				if(!userDto.Name) { return userDto.text; }
				var $userDto = $(
					'<div>' + userDto.Name + '</div>'
					);
				return $userDto;
			}
		});
}

var bindDataTable = function()
{
	// kickstart jQuery DataTable
    var table = $('#example').DataTable({
    	"ajax": '/Home/SubmittedTasks',
    	"order": [[ 3, "desc" ]],
    	"columnDefs": [{
		    "defaultContent": "-",
		    "targets": "_all"
		  },
		  //{
		  //	"targets": [0],
		  //	"data": "Id"
		  //},
		  {
		  	"targets": [0],
		  	"data": "Name"
		  },
		  {
		  	"targets": [1],
		  	"data": "Type"
		  },
		  {
		  	"targets": [2],
		  	"data": "Status"
		  },
		  {
		  	"targets": [3],
		  	"data": "CreatedDate"
		  },
		  {
		  	"targets": [4],
		  	"data": "CreatedBy"
		  },
		  {
		  	"targets": [5],
		  	"data": "FileURL",
		  	"render": function( data, type, row ) {
		  		return "<a href=" + data +">Download</a>";
		  	}
		  }]
    });

 	// Refresh every x seconds
	setInterval( function () {
    	table.ajax.reload( null, false ); // user paging is not reset on reload
    	vm.Notifications.add("Table data has been refreshed", "info", 0);
	}, 120000 ); // 2-minutes
};




function singleFileSelected(evt) {
    //var selectedFile = evt.target.files can use this  or select input file element 
    //and access it's files object
    var selectedFile = ($("#FileToUpload"))[0].files[0];//FileControl.files[0];
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

function UploadFile() 
{
	// 
    // Build our "Form Data" object
    // https://developer.mozilla.org/en-US/docs/Web/API/FormData/Using_FormData_Objects
    // https://developer.mozilla.org/en-US/docs/Web/API/FormData/append
    var formData = new FormData();
    var TaskType = $("#TaskType").val();
    var SubmittedBy = $('#SubmittedBy').val();
	var TaskName = $("#TaskName").val();
	var TaskDescription = $("#TaskDescription").val();
    var fileToUpload = $('#FileToUpload').prop('files')[0];


    // Validation
    if(TaskType != "PayMIP" && TaskType != "PostTransactions" && TaskType != "CreateWarehouseLineFile")
    {
    	var message = "An unsupported Task Type is selected.";
    	vm.Notifications.add(message, "error", 0);
    	return;
    }

    if(SubmittedBy == null || isNaN(parseInt(SubmittedBy))){
    	var message = "You must select a user before submitting a task.";
    	vm.Notifications.add(message, "error", 0);
    	return;
    }

    if(TaskName == ""){
    	var message = "The Task Name field cannot be left empty.";
    	vm.Notifications.add(message, "error", 0);
    	return;
    }

    if(TaskDescription == ""){
    	var message = "The Task Description field cannot be left empty.";
    	vm.Notifications.add(message, "error", 0);
    	return;
    }

    if(!fileToUpload){
    	var message = "You must select a file to upload before submitting a task.";
    	vm.Notifications.add(message, "error", 0);
    	return;
    }


    // Add to form
    formData.append("SubmittedById", SubmittedBy);
    formData.append("Type", TaskType);
    formData.append("Name", TaskName);
    formData.append("Description", TaskDescription);
    formData.append("fileToUpload", fileToUpload, fileToUpload.name);
    
    //
    // POST
    //
    $.ajax({
        url: '/Home/SubmitTask',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                //myXhr.upload.onprogress = progressHandlingFunction
                // For handling the progress of the upload
                // todo: implement? Maybe only for image and more complex file types
                //myXhr.upload.addEventListener('progress', progressHandlingFunction, false);
            }
            return myXhr;
        },
        //Ajax events
        success: function(data)
        { 
        	// Refresh table
        	$('#example').DataTable().ajax.reload();
        	// Close pane
        	$('.modal').trigger("click");
        	// Clear pane's form fields
			vm.Form.clear();
        	// Toast notification
        	vm.Notifications.add(data.Message, "success", 0);
        	console.log(data); 
        },
        error: function(data){ 
        	// Toast notification
        	console.log(data); 
        },
        //complete: function(data){ console.log("complete"); console.log(data); },
        // Form data
        data: formData,
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