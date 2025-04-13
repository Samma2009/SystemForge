(block) => {
    const X = Gen.valueToCode(block, 'X', javascript.Order.ATOMIC);
    const Y = Gen.valueToCode(block, 'Y', javascript.Order.ATOMIC);

    const code = `fb_ptr[(${Y}) * (framebuffer->pitch / 4) + (${X})]`;
    return [code,javascript.Order.ATOMIC];
}