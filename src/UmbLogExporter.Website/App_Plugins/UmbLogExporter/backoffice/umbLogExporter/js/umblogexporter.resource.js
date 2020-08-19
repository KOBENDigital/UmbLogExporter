function umbLogExporterResource($http, notificationsService) {

	return {
		exportLogs(vm, options) {

			var defaults = {
				orderDirection: "Descending",
				filterExpression: ''
			};

			//overwrite the defaults if there are any specified
			angular.extend(defaults, options);

			//now copy back to the options we will use
			options = defaults;

			vm.exporting = true;

			$http({
				method: 'GET',
				url: '/Umbraco/backoffice/UmbLogExporter/LogExporter/Export/',
				params: options,
				responseType: 'arraybuffer'
			}).then(function (response) {

				var headers = response.headers();

				var contentDispositionHeader = headers['content-disposition'];
				var filename = contentDispositionHeader.split(';')[1].trim().split('=')[1].replace(/"/g, '');
				var contentType = headers['content-type'];

				var linkElement = document.createElement('a');
				try {
					var blob = new Blob([response.data], { type: contentType });
					var url = window.URL.createObjectURL(blob);

					linkElement.setAttribute('href', url);
					linkElement.setAttribute("download", filename);

					var clickEvent = new MouseEvent("click", {
						"view": window,
						"bubbles": true,
						"cancelable": false
					});
					linkElement.dispatchEvent(clickEvent);

					vm.exporting = false;
					notificationsService.success('Export successful', "the file '" + filename + "' is downloading");
				} catch (ex) {
					vm.exporting = false;
					notificationsService.error('Export unsuccessful', 'problem downloading file');
				}
			}, function (response) {
				vm.exporting = false;
				notificationsService.error('Export unsuccessful', 'server error, please check the log for details');
			});
		}
	};
}
angular.module('umbraco.resources').factory('umbLogExporterResource', umbLogExporterResource);