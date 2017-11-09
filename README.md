# stackTraceangelo
*StackTraceangelo* is the World's first [Stack Trace Art](http://www.thehumbleprogrammer.com/stack-trace-art) editor. (To my knowledge at least.) Its name is the combination of the term [Stack Trace](https://en.wikipedia.org/wiki/Stack_trace) and the name of the famous renaissance artist [Michelangelo Buonarroti](https://en.wikipedia.org/wiki/Michelangelo).

[![AppVeyor](https://img.shields.io/appveyor/ci/ironcev/stackTraceangelo.svg)](https://ci.appveyor.com/project/ironcev/stackTraceangelo)

## About
Stack Trace Art is the art of throwing an exception that creates a beautiful drawing on the caller's stack trace. You can read more about it in my [original blog post on Stack Trace Art](http://www.thehumbleprogrammer.com/stack-trace-art).

The pieces of the finest Stack Trace Art given below should give you an idea of what Stack Trace Art is and what kind of exceptions you can easily create with *stackTraceangelo*. All those pieces of art are taken from the [*stackTraceangelo* Art Gallery](/Source/ArtGallery/README.md).

### Лулу и как се прави дъга
This nostalgic childhood image was inspired by a beautiful Bulgarian poem found in [Petya Kokudeva's book "Lulu"](http://www.dailymotion.com/pkokudeva#video=xm47k7).

![Лулу и как се прави дъга](Source/ArtGallery/LuluIKakSePraviDaga.png)

### Keeping My Fingers Crossed
Here is an artistic way to tell your colleagues that you are keeping your fingers crossed for them.

![Keeping my fingers crossed](Source/ArtGallery/CrossedFingers.png)

### Let's Meditate Together
Meditating together on the [sacred sound of ऊँ (Om)](https://en.wikipedia.org/wiki/Om) gets a different dimension when accompanied with the `NestedOmException`.

![Nested Om (ऊँ)](Source/ArtGallery/NestedOm.png)

### Good Job!
Thinking of a unique way of congratulating your team members on a good job?

![Good Job!](Source/ArtGallery/GoodJob.png)

### Be Careful The Cat
Some pieces of wisdom ar worth repeating over and over again. [Be careful the cat!](http://www.youtube.com/watch?v=tPAJomPCdZs).

![Be careful the cat!](Source/ArtGallery/TheCatInTheSac.png)

## How Does It Work?
How Stack Trace Art works? Are those exceptions really *real* exceptions or a some kind of a fake? Out of my experience, programmer's first reaction on Stack Trace Art, and I witnessed it many times, is mostly disbelief. "These cannot be real method calls. Do you rewrite the stack trace information somehow?" I do not :-) Stack Trace Art exceptions are genuine, regular, real programming exceptions. No tricks of any kind ;-)

To understand the "magic" behind the Stack Trace Art exceptions [read this blog post: His Majesty, Hangul the Filler](http://thehumbleprogrammer.com/his-majesty-hangul-the-filler/).

To understand the inner mechanics of the *stackTraceangelo* [read this blog post: Clarke's Third Law Exception - Step by Step](http://thehumbleprogrammer.com/clarkes-third-law-exception-step-by-step/).

## Rekindle Your Artistic Soul
Stack Trace Art is all about rekindling our artistic programmer souls. Although being in an early proof-of-concept stage, *stackTraceangelo* is already mature enough to help you draw your own pieces of Stack Trace Art. In other words, to help you rekindle your artistic soul. So, download it, create some beautiful pieces of Stack Trace Art and inject them into other programmer's code!

You still do not feel ready to start creating your own Stack Trace Art? Don't worry. Every true artist experiences the Artist's Block. To overcome the block you can seek for inspiration at the [*stackTraceangelo* Art Gallery](/Source/ArtGallery/README.md). Injecting some of the existing pieces of Stack Trace Art into your colleagues' code is a great way to [overcome the Artist's Block](https://www.wikihow.com/Overcome-Artist%27s-Block) ;-)