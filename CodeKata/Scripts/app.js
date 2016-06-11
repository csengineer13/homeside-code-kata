$(function() {


	vm.init();

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

});



var vm = {};

vm.init = function()
{
	bindClickEvents();
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