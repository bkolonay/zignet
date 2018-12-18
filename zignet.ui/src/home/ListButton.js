import React, { Component } from 'react';

class ListButton extends Component {

  render() {
    const button = this.props.grouped ? 
      <a href="/" className="btn btn-outline-primary" role="button" title="Click to ungroup results">Ungroup</a> :
      <a href="/?group=true" className="btn btn-primary" role="button" title="Click to group results by environment">Group</a>;

    return (
      button
    );
  }
}

export default GroupSuitesButton;
