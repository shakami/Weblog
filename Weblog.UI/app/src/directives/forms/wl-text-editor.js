(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlTextEditor', wlTextEditor);

    function wlTextEditor() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/forms/wl-text-editor.html',
            scope: {
                type: '@',
                model: '='
            },
            controller: function ($scope, textEditorService) {
                switch ($scope.type) {
                    case 'post':
                    default:
                        $scope.editorOptions = textEditorService.postOptions();
                        break;
                    case 'comment':
                        $scope.editorOptions = textEditorService.commentOptions();
                        break;
                }
            }
        };
    }

})();
