import './home.css';

export default function Home() {
    return (
        <div className="home">
            <h1>Welcome to Disparonote</h1>
            <div className="instructions-container">
                <img src="/images/note-sender.png" alt="Note Sender" className="note-sender-img" />
                <p>Send self-destructing notes securely and privately.</p>
                <p>Click <strong>"Create Note"</strong> to generate a new note.</p>
                <p>Share the generated link with anyone you want.</p>
                <p>Once viewed, the note will be deleted forever.</p>
                <p>To create a new note, you must <strong>register</strong> and <strong>log in</strong></p>
                <img src="/images/note-reciever.png" alt="Note Reciever" className="note-reciever-img" />
            </div>
        </div>
    );
}
