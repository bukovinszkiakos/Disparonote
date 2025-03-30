'use client';
import './home.css';
import Image from 'next/image';

export default function Home() {
    return (
        <div className="home">
            <br />
            <h1>Welcome to Disparonote</h1>
            <br />
            <br />
            <div className="instructions-container">
                <Image
                    src="/images/note-sender.png"
                    alt="Note Sender"
                    className="note-sender-img"
                    width={100}
                    height={100}
                />
                <p>Send self-destructing notes securely and privately.</p>
                <p>Click <strong>&quot;Create Note&quot;</strong> to generate a new note.</p>
                <p>Share the generated link with anyone you want.</p>
                <p>Once viewed, the note will be deleted forever.</p>
                <p>To create a new note, you must <strong>register</strong> and <strong>log in</strong></p>
                <Image
                    src="/images/note-reciever.png"
                    alt="Note Receiver"
                    className="note-reciever-img"
                    width={100}
                    height={100}
                />
            </div>
        </div>
    );
}
