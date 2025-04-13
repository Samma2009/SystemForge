(block) => {
    var functionName = block.getFieldValue('NAME').replace(' ','_').replace('.','__').replace(':','___');
    var code = functionName + '()';
    return [code, javascript.Order.FUNCTION_CALL];
}  