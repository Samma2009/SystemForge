(block) => {
    var functionName = block.getFieldValue('NAME').replace(' ','_').replace('.','__').replace(':','___');
    var code = functionName + '();\n';
    return code;
}  