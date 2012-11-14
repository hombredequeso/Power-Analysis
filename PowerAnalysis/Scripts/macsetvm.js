window.HDQ.namespace('HDQ.macsetvm');

window.HDQ.macsetvm = (function () {
	return {
		create: function (initData) {
			var vm = ko.mapping.fromJS(initData);
			vm.elecPerMw = ko.observable(10);
			
			var itemCount = vm.items().length;
			var vmItems = vm.items();
			for (var i = 0; i < itemCount; i++) {
				vmItems[i].xCalc = ko.computed(function () {
					var xPreparsed = this.x().toString();
					var xParsed = xPreparsed.replace( /e/g , vm.elecPerMw());
					return eval(xParsed);
				}, vmItems[i]);
				vmItems[i].yCalc = ko.computed(function () {
					var yPreparsed = this.y().toString();
					var yParsed = yPreparsed.replace( /e/g , vm.elecPerMw());
					return eval(yParsed);
				}, vmItems[i]);
			}
			return vm;
		}
	};
}());