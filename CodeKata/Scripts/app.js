$(function() {


	vm.init();

    // kickstart jQuery DataTable
    $('#example').DataTable();

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