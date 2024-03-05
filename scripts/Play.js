var config = {
    type: Phaser.AUTO,
    width: 1142,
    height: 600,
    physics: {
        default: 'arcade',
        arcade: {
            gravity: { y: 800 },
            debug: false
        }
    },
    scene: {
        preload: preload,
        create: create,
        update: update
    }
};

var player;
var stars;
var traps;
var platforms;
var cursors;
var score = 0;
var deaths = 0;
var timeText;
var gameOver = false;
var entrance = true;
var win = false;
var scoreText;
var deathCounter;
var portNum;
var portNum2;
var portalEmpty;
var portalFull;
var portals;
var stopThis = false;
var timeout1;
var timemout2;
var MAPTYPE = document.getElementById("MAPTYPE");
var STARX = document.getElementById("STARX").value.split("_");
var STARY = document.getElementById("STARY").value.split("_");
var TRAPX = document.getElementById("TRAPX").value.split("_")
var TRAPY = document.getElementById("TRAPY").value.split("_");
var TRAPTYPE = document.getElementById("TRAPTYPE").value.split("_");
var game = new Phaser.Game(config);
const self = this;
var canJump = true;
var gameRuntime;

function preload() {
    this.load.image('cyberpunk', 'assets/sets/cyberpunk-street.png');
    this.load.image('steampunk', 'assets/sets/city.jpeg');
    this.load.image('blank', 'assets/sets/west.jpg');
    this.load.spritesheet('saw', 'assets/sprites/saws.png', { frameWidth: 548, frameHeight: 548 });
    this.load.image('star', 'assets/sprites/star.png');
    this.load.image('bluePortal', 'assets/sprites/portal.png');
    this.load.image('ground1', 'assets/sets/objects/platform1.png');
    this.load.image('ground2', 'assets/sets/objects/platform2.png');
    this.load.image('ground3', 'assets/sets/objects/platform3.png');
    this.load.image('ground4', 'assets/sets/objects/brownBlock.png');
    this.load.image('ground5', 'assets/sets/objects/blackBlock.png');
    this.load.image('ver-tile', 'assets/sets/objects/ver-tile.png');
    this.load.image('hor-tile-med', 'assets/sets/objects/hor-tile-med.png');
    this.load.image('hor-tile-big', 'assets/sets/objects/hor-tile-big.png');
    this.load.image('hor-tile-small', 'assets/sets/objects/hor-tile-small.png');
    this.load.spritesheet('portalEmpty', 'assets/sprites/portalRings1.png', { frameWidth: 32, frameHeight: 32 });
    this.load.spritesheet('portalFull', 'assets/sprites/portalRings2.png', { frameWidth: 32, frameHeight: 32 });
    this.load.spritesheet('dude', 'assets/sprites/dude.png', { frameWidth: 32, frameHeight: 48 });
}

function create() {
    platforms = this.physics.add.staticGroup();
    switch (MAPTYPE.value) {
        case "blank":
            bg = this.add.image(375, 250, 'blank').setScale(0.52);
            platforms.create(150, 250, 'ground2');
            platforms.create(600, 460, 'ground3');
            platforms.create(600, 175, 'ground2');
            platforms.create(1000, 175, 'ground2');
            platforms.create(300, 400, 'ground1')
            platforms.create(0, 600, 'ground4');
            platforms.create(500, 600, 'ground4');
            break;
        case "cyberpunk":
            bg = this.add.image(240, 280, 'cyberpunk').setScale(3);
            platforms.create(150, 250, 'hor-tile-med');
            platforms.create(600, 500, 'ver-tile');
            platforms.create(600, 175, 'hor-tile-big');
            platforms.create(1000, 175, 'ver-tile');
            platforms.create(300, 400, 'hor-tile-small')
            platforms.create(0, 600, 'ground5');
            platforms.create(500, 600, 'ground5');
            break;
        case "steampunk":
            bg = this.add.image(375, 250, 'steampunk');
            platforms.create(150, 250, 'ground2');
            platforms.create(600, 460, 'ground3');
            platforms.create(600, 175, 'ground2');
            platforms.create(1000, 175, 'ground2');
            platforms.create(300, 400, 'ground1')
            platforms.create(0, 600, 'ground4');
            platforms.create(500, 600, 'ground4');
            break;
    }
    for (let i = 0; i < TRAPX.length; i++) {
        if (TRAPTYPE[i] === "portalEmpty") {
            portalEmpty = this.add.sprite(TRAPX[i], TRAPY[i], 'portalEmpty').setScale(2);
            portNum = i;
        }
        if (TRAPTYPE[i] === "portalFull") {
            portalFull = this.add.sprite(TRAPX[i], TRAPY[i], 'portalFull').setScale(2)
            portNum2 = i;
        }
    }
    player = this.physics.add.sprite(TRAPX[portNum] - 10, TRAPY[portNum] - 10, 'dude');
    //this.physics.world.enable(portalFull);
    portals = this.physics.add.staticGroup();
    portals.create(TRAPX[portNum2], TRAPY[portNum2], 'portalFull').setScale(2);
    //player.setBounce(0.2);
    player.setCollideWorldBounds(false);
    this.anims.create({
        key: 'left',
        frames: this.anims.generateFrameNumbers('dude', { start: 0, end: 3 }),
        frameRate: 10,
        repeat: -1
    });
    this.anims.create({
        key: 'turn',
        frames: [{ key: 'dude', frame: 4 }],
        frameRate: 20
    });
    this.anims.create({
        key: 'right',
        frames: this.anims.generateFrameNumbers('dude', { start: 5, end: 8 }),
        frameRate: 10,
        repeat: -1
    });
    this.anims.create({
        key: 'entry',
        frames: this.anims.generateFrameNumbers('portalEmpty', { start: 0, end: 16 }),
        frameRate: 15,
        repeat: 1
    });
    this.anims.create({
        key: 'exit',
        frames: this.anims.generateFrameNumbers('portalFull', { start: 0, end: 4 }),
        frameRate: 15
    });
    this.anims.create({
        key: 'spin',
        frames: this.anims.generateFrameNumbers('saw', { start: 0, end: 4 }),
        frameRate: 20
    });
    cursors = this.input.keyboard.createCursorKeys();
    stars = this.physics.add.group();
    //Here, split all the stars' positions from the textbox, and push the new stars to the physics group
    for (let i = 0; i < STARX.length; i++) {
        stars.create(Number(STARX[i]), Number(STARY[i]), 'star');
    }
    stars.children.iterate(function (child) {
        child.body.setAllowGravity(false);
        child.setScale(0.5);
    });
    traps = this.physics.add.group();
    //Here, split all the traps' positions and types from the textbox, and push the new traps to the physics group
    for (let i = 0; i < TRAPX.length; i++) {
        if(TRAPTYPE[i] != "portalEmpty" && TRAPTYPE[i] != "portalFull")
            traps.create(Number(TRAPX[i]), Number(TRAPY[i]), TRAPTYPE[i]);
    }
    traps.children.iterate(function (trap) {
        trap.body.setAllowGravity(false);
        if (trap.texture.key === "saw") {
            trap.setScale(0.15);
            trap.setCollideWorldBounds(true);
            goLeft(0);
            function goLeft(counter) {
                if (!stopThis) {
                    trap.setVelocityX(-200);
                    setTimeout(goRight, 2000, counter);
                }
            }
            function goRight(counter) {
                if (!stopThis) {
                    trap.setVelocityX(200);
                    setTimeout(goLeft, 2000, (counter + 1) % 4);
                }
            }
        }
    });
    scoreText = this.add.text(16, 16, 'Stars collected: 0', { fontSize: '14px', fill: '#000' });
    deathCounter = this.add.text(16, 32, 'Deaths: 0', { fontSize: '14px', fill: '#000' });
    timeText = this.add.text(16, 48, "Time Elapsed: ", { fontSize: '14px', fill: '#000' });
    this.physics.add.collider(player, platforms);
    this.physics.add.collider(traps, platforms);
    this.physics.add.overlap(player, stars, collectStar, null, this);
    this.physics.add.overlap(player, portals, Victory, null, this);
    this.physics.add.overlap(player, traps, hitTrap, null, this);
}


let vx = 0;
function update(time) {
    gameRuntime = time * 0.001 - 1; //Converted to Seconds
    timeText.setText("Time Elapsed: " + Math.round(gameRuntime) + " s");
    if (player.x < 10 || player.x > 1132) {
        vx = 0;
        player.setVelocityX(vx);
        if (player.x > 1132)
            player.x = 1130;
        else player.x = 12;
    }
    else {
        if (vx <= 200 && vx >= -200) {
            if (cursors.left.isDown) {
                if (vx > -30 && vx < 20) {
                    vx = -30;
                }
                if (vx > 0)
                    vx -= 20;
                vx -= 3;
                player.setVelocityX(vx);
                player.anims.play('left', true);
            }
            else if (cursors.right.isDown) {
                if (vx < 30 && vx > -20) {
                    vx = 30;
                }
                if (vx < 0)
                    vx += 20;
                vx += 3;
                player.setVelocityX(vx);
                player.anims.play('right', true);
            }
        }
        if (!cursors.left.isDown && !cursors.right.isDown) {


            if (vx > 0) {
                vx -= 20;
                if (vx < 0)
                    vx = 0;
            }
            if (vx < 0) {
                vx += 20;
                if (vx > 0)
                    vx = 0;
            }
            player.setVelocityX(vx);;

            player.anims.play('turn');
        }
    }

    if (gameOver) {
        entrance = true;
        var tempTrapNum = 0;
        traps.children.iterate(function (trap) {
            for (let i = tempTrapNum; i < TRAPX.length; i++) {
                if (TRAPTYPE[i] === "saw") {
                    trap.x = Number(TRAPX[i]);
                    trap.y = Number(TRAPY[i]);
                    trap.setVelocityX(0);
                    goLeft(0);
                    function goLeft(counter) {
                        trap.setVelocityX(-200);
                        timeout1 = setTimeout(goRight, 2000, counter);
                    }
                    function goRight(counter) {
                        trap.setVelocityX(200);
                        timeout2 = setTimeout(goLeft, 2000, (counter + 1) % 4);
                    }
                    tempTrapNum = i + 1;
                    i = TRAPX.length;
                }
            }
        });
        player.setVelocityY(0);
        this.physics.resume();
        gameOver = false;
    }
    
    if (cursors.up.isDown && player.body.touching.down && canJump)
        player.setVelocityY(-500);
    if (entrance) {
        portalEmpty.anims.play('entry', true);
        entrance = false;
    }
    if (!win)
        portalFull.anims.play('exit', true);
    traps.children.iterate(function (trap) {
        if (trap.texture.key === "saw") {
            trap.anims.play('spin', true);
            sleep(10000);
        }
    });
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
    //sleep(2000).then(() => { console.log('World!'); });
}

function collectStar(player, star) {
    star.disableBody(true, true);
    score ++;
    scoreText.setText('Stars collected: ' + score);
    canJump = false;
    setTimeout(() => { canJump = true; }, 1);
}

function hitTrap(player, trap) {
    stopThis = true;
    try {
        clearTimeout(timeout1);
        clearTimeout(timeout2);
    }
    catch { /*console.timeLog();*/}
    deaths++;
    deathCounter.setText('Deaths: ' + deaths);
    this.physics.pause();
    document.onkeypress = function (event) {
        restart(event);
    };
}

function restart(event) {
    if (event.key === 'r') { // 'r' iskeyCode 82
        player.x = TRAPX[portNum] - 2;
        player.y = TRAPY[portNum] - 2;
        gameOver = true;
    }
}


function Victory(player) {
    if (!win) {
        win = true;
        portalFull.anims.play('entry');
        gameEnd();
    }
}

function gameEnd() {
    $.ajax({
        type: "POST",
        url: "Play.aspx/GameEnd",
        data: JSON.stringify({ timeElapsed: Math.round(gameRuntime), starsCollected: score, deathCount: deaths, victory: win }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response.d);
            window.location.href = "Endscreen.aspx";
        },
        error: function (error) {
            console.log("Error: " + error.responseText);
        }
    });
}

function confirmDiscard() {
    if (confirm("Are you sure?"))
        gameEnd();
}