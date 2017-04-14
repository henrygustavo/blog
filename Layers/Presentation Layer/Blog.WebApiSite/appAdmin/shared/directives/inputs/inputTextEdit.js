angular.module("angular-input-text", [])
    .provider("inputTextConfig",
        function () {

            this.setValidationMessagePath = function (validationMessagePath) {
                this.validationMessagePath = validationMessagePath;
            };

            this.$get = function () {
                return this;
            };

        })
    .directive("inputTextEdit",
        function (inputTextConfig) {
            return {
                restrict: "E",
                replace: true,
                require: "ngModel",
                compile: function (element, attributes) {

                    $(element).html(createEditInput(attributes, inputTextConfig.validationMessagePath));
                }
            };
        });


function createEditInput(attributes, inputTextEditConfig) {

    var type = "T";
    var form = "form";
    var name = "";
    var select = "";
    var attributesResult = "";
    var hasValidation = false;
    var updateOnBlur = '  ng-model-options="{updateOn: ' + "'blur'" + '}" ';
    var label = "";
    var typeInput = ' type="text" ';

    var finalHtmlInput = "";
    var input = "";
    var formGroupIni = '<div class="form-group">';
    var formGroupEnd = "</div>";


    var validationMessagesIni = "";
    var validationMessagesEnd = "";

    var datehtmlIni = "";
    var datehtmlEnd = "";

    angular.forEach(attributes.$attr,
        function (val, key) {
            var value = attributes[key];
            switch (key) {
            case "label":
                label = "<label>" + value + "</label>";
                break;

            case "form":
                form = value;
                break;

            case "name":
                attributesResult += ' name="' + value + '" ng-model="model.' + value + '" ';
                name = value;
                break;

            case "maxlength":
                attributesResult += ' ng-maxlength="' + value + '" ';
                hasValidation = true;
                break;

            case "minlength":
                attributesResult += ' ng-minlength="' + value + '" ';
                hasValidation = true;
                break;

            case "required":
                attributesResult += attributes.select != undefined ? " ng-required-select " : " required ";
                hasValidation = true;
                break;

            case "password":
                typeInput = ' type="password" ';
                break;

            case "email":
                typeInput = ' type="email" ';
                hasValidation = true;
                break;

            case "decimal":
                attributesResult += ' fcsa-number="{ preventInvalidInput:true, maxDecimals: ' + value + '}" ';
                updateOnBlur = '  ng-model-options="{updateOn: ' + "'change blur'" + '}" ';
                hasValidation = true;
                break;

            case "mask":
                attributesResult += ' ui-mask="' + value + '" ';
                updateOnBlur = "";
                break;

            case "date":
                if (attributes.readonly == undefined) {
                    datehtmlIni = '<p class="input-group">';
                    updateOnBlur = "";
                    attributesResult += ' uib-datepicker-popup="dd/MM/yyyy" is-open="' + name + 'Opened" ';
                    datehtmlEnd =
                        '<span class="input-group-btn"><button type="button" class="btn btn-default" ng-click="' +
                        name +
                        'Open($event)"><i class="glyphicon glyphicon-calendar"></i></button></span></p>';
                }
                break;

            case "disabled":
                attributesResult += ' disabled="disabled" ';
                break;

            case "readonly":
                if (attributes.editor != undefined) {

                    attributesResult += ' ta-target-toolbars="toolbar" ta-disabled="true" ';
                } else {
                    attributesResult += ' readonly="readonly" ';
                }
                break;

            case "checkbox":
                updateOnBlur = "";
                type = "CB";
                break;

            case "select":
                updateOnBlur = "";
                type = "S";
                select = value;
                break;

            case "area":
                type = "A";
                attributesResult += ' rows="' + value + '" ';
                break;

            case "editor":
                type = "E";
                label = "";
                updateOnBlur = "";
                break;

            default:
                if (value == "" || value == undefined) {
                    attributesResult += " " + val + " ";
                } else {
                    attributesResult += " " + val + '="' + value + '" ';
                }
                break;
            }
        });

    attributesResult += updateOnBlur;

    if (hasValidation) {

        validationMessagesIni += '<div ng-class="{' +
            "'has-error'" +
            ": " +
            form +
            "." +
            name +
            ".$dirty && " +
            form +
            "." +
            name +
            '.$invalid}">';
        validationMessagesEnd += '<div class="text-danger" ng-messages="' +
            form +
            "." +
            name +
            '.$error" ng-if="' +
            form +
            "." +
            name +
            '.$dirty">' +
            '<div ng-messages-include="' +
            inputTextEditConfig +
            '"></div></div>';
    }

    switch (type) {

    case "A":
        input = '<textarea class="form-control" ' + attributesResult + "></textarea>";
        break;

    case "CB":
        formGroupIni += '<div class="checkbox" style="margin-left: 19px">';

        input = '<input name="' + name + '" type="checkbox" ng-model="model.' + name + '" ' + attributesResult + " >";

        formGroupEnd += "</div>";
        finalHtmlInput = formGroupIni + input + attributes.label + formGroupEnd;
        return finalHtmlInput;
    case "E":
        input = ' <div text-angular="text-angular" ' + attributesResult + "></div> ";
        break;

    case "S":
        input = '<select class="form-control" ' + attributesResult + ' ng-options="' + select + '"></select>';
        break;

    case "T":
        input = "<input " + typeInput + ' class="form-control" ' + attributesResult + " />";
        break;
    }

    finalHtmlInput = formGroupIni +
        label +
        validationMessagesIni +
        datehtmlIni +
        input +
        datehtmlEnd +
        validationMessagesEnd +
        formGroupEnd +
        "</div>";

    return finalHtmlInput;
}

//Example
//<input-text-edit label="Name" name="name"  minlength ="2"  maxlength="10" ></input-text-edit>
//<input-text-edit label="Icon" name="icon"  required></input-text-edit>
//<input-text-edit label="Email" name="email"  email ></input-text-edit>
//<input-text-edit label="Mask" name="mask"  mask="(51) 9?9999-9999" ></input-text-edit>
//<input-text-edit label="Decimal" name="decimal" decimal="2" ></input-text-edit> 
//<input-text-edit label="Area" name="area" area="5" ></input-text-edit>
//<input-text-edit label="readonly" name="readonly" readonly ></input-text-edit>
//<input-text-edit label="Date" name="date" date required></input-text-edit>
//<input-text-edit label="State" name="state" select="state.value as state.text for state in states" required ></input-text-edit>
//<input-text-edit label="Description" name="description" editor required></input-text-edit>