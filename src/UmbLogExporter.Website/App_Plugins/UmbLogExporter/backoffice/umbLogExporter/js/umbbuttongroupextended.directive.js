(function () {
	'use strict';

	function buttonGroupExtendedDirective() {

		function link(scope) {

			scope.dropdown = {
				isOpen: false
			};

			scope.toggleDropdown = function () {
				scope.dropdown.isOpen = !scope.dropdown.isOpen;
			};

			scope.closeDropdown = function () {
				scope.dropdown.isOpen = false;
			};

			scope.executeMenuItem = function (subButton) {
				subButton.handler(scope.args);
				scope.closeDropdown();
			};

		}

		var directive = {
			restrict: 'E',
			replace: true,
			templateUrl: '/App_Plugins/UmbLogExporter/backoffice/umbLogExporter/directives/umb-button-group-extended.html',
			scope: {
				defaultButton: "=",
				subButtons: "=",
				state: "=?",
				direction: "@?",
				float: "@?",
				buttonStyle: "@?",
				size: "@?",
				icon: "@?",
				label: "@?",
				labelKey: "@?",
				args: "@?"
			},
			link: link
		};

		return directive;
	}

	angular.module('umbraco.directives').directive('umbButtonGroupExtended', buttonGroupExtendedDirective);

})();