$(document).ready(function () {
	//$("#IngredientSelection").val(["5", "6"]).select2();
	$("#IngredientSelection").select2({
		multiple: true,
		placeholder: "Select Ingredients",
		allowClear: true
		//initSelection: function (element, callback) {
		//	var data = [];
		//	data.push({ id: 6, text: 5 });
		//	callback(data);
		//}


	});
	//var data = ['3', '4'];
	//$("#IngredientSelection").val(data);

	//$("#IngredientSelection").trigger('change');

	$("#RecipeSelection").select2({
		multiple: true,
		placeholder: "Select Recipes",
		allowClear: true
	});

	$("#MealSelection").select2({
		multiple: true,
		placeholder: "Select Meals",
		allowClear: true
	});

	$('a[data-toggle="tab"]').click(function (e) {
		e.preventDefault();
		$(this).tab('show');
	});

	$('a[data-toggle="tab"]').on("shown.bs.tab", function (e) {
		var id = $(e.target).attr("href");
		localStorage.setItem('selectedTab', id)
	});

	var selectedTab = localStorage.getItem('selectedTab');
	if (selectedTab != null) {
		$('a[data-toggle="tab"][href="' + selectedTab + '"]').tab('show');
	}
});