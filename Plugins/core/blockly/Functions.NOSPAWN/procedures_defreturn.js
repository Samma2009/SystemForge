(block) => {
    var functionName = block.getFieldValue('NAME').replace(' ','_').replace('.','__').replace(':','___');

    var ret = Gen.valueToCode(block, 'RETURN', javascript.Order.NONE);
    var rettype = "void";

    rettype = typeof ret;

    if (!isNaN(Number.parseFloat(ret))) {
        rettype = "number";
        ret = Number.parseFloat(ret);
    }

    if (rettype == "number") {
        if (Number.isInteger(ret)) {
            rettype = "int";
        }
        else {
            rettype = "float";
        }
    }

    if (workspace.getVariable(`${ret}`,"char*") != null) rettype = "char*";
    if (workspace.getVariable(`${ret}`,"float") != null) rettype = "float";
    if (workspace.getVariable(`${ret}`,"int") != null) rettype = "int";

    var code = `${rettype} ${functionName}() {\n`;
    var nextBlock = block.getInputTargetBlock('STACK');
    while (nextBlock) {
        var tempCode = Gen.blockToCode(nextBlock);
        if (Gen.STATEMENT_PREFIX) {
            tempCode = Gen.prefixLines(
                Gen.STATEMENT_PREFIX.replace(/%1/g,
                    '\'' + nextBlock.id + '\'') + '\n', Gen.INDENT) + tempCode;
        }
        code += tempCode;
        nextBlock = nextBlock.getNextBlock();
    }
    code += `return ${ret};\n}\n`;
    return code;
}