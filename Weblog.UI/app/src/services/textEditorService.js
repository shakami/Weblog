(function () {

    'use strict';

    angular
        .module('app')
        .value('froalaConfig', froalaConfig())
        .factory('textEditorService', textEditorService);

    function textEditorService() {
        return {
            postOptions: postOptions,
            commentOptions: commentOptions
        };

        function postOptions() {
            return {
                placeholderText: 'Body',
                toolbarButtons: {
                    'moreText': {
                        'buttons': ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'clearFormatting'],
                        'buttonsVisible': 2
                    },
                    'moreParagraph': {
                        'buttons': ['formatOLSimple', 'alignLeft', 'alignCenter', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote'],
                        'buttonsVisible': 1
                    },
                    'moreRich': {
                        'buttons': ['insertLink', 'emoticons', 'insertHR', 'insertImage', 'insertVideo', 'insertTable', 'specialCharacters']
                    },
                    'moreMisc': {
                        'buttons': ['undo', 'redo', 'fullscreen', 'print', 'getPDF', 'spellChecker', 'selectAll', 'help'],
                        'align': 'right',
                        'buttonsVisible': 2
                    }
                }
            };
        }

        function commentOptions() {
            return {
                placeholderText: 'Type your comment here...',
                toolbarButtons: ['bold', 'italic', 'formatOLSimple', 'insertLink', 'emoticons', 'insertImage', 'undo', 'redo', 'spellChecker']
            };
        }
    }

    function froalaConfig() {
        return {
            quickInsertEnabled: false,
            imagePaste: false,
            imageUpload: false,
            videoUpload: false,
            fileUpload: false
        }
    };

})();
